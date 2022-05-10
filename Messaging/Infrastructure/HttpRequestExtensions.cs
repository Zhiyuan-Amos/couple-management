using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Couple.Messaging.Infrastructure;

// https://www.tomfaltesek.com/azure-functions-input-validation/
public static class HttpRequestExtensions
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    ///     Returns the deserialized request body with validation information.
    /// </summary>
    /// <typeparam name="T">Type used for deserialization of the request body.</typeparam>
    /// <typeparam name="TV">
    ///     Validator used to validate the deserialized request body.
    /// </typeparam>
    /// <param name="request"></param>
    /// <returns></returns>
    public static async Task<ValidatableRequest<T>> GetJsonBody<T, TV>(this HttpRequest request)
        where TV : AbstractValidator<T>, new()
    {
        var requestBody = await request.ReadAsStringAsync();
        var requestObject = JsonSerializer.Deserialize<T>(requestBody, Options);
        var validator = new TV();
#pragma warning disable CS8604
        var validationResult = await validator.ValidateAsync(requestObject);
#pragma warning restore CS8604

        if (!validationResult.IsValid)
        {
            return new()
            {
                Value = requestObject, Json = requestBody, IsValid = false, Errors = validationResult.Errors
            };
        }

        return new() { Value = requestObject, Json = requestBody, IsValid = true };
    }
}

public static class ValidationExtensions
{
    /// <summary>
    ///     Creates a <see cref="BadRequestObjectResult" /> containing a collection
    ///     of minimal validation error details.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static BadRequestObjectResult ToBadRequest<T>(this ValidatableRequest<T> request) =>
        new(request.Errors.Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage }));
}

public class ValidatableRequest<T>
{
    /// <summary>
    ///     The deserialized value of the request.
    /// </summary>
#pragma warning disable CS8618
    public T Value { get; set; }
#pragma warning restore CS8618

#pragma warning disable CS8618
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string Json { get; set; }
#pragma warning restore CS8618

    /// <summary>
    ///     Whether or not the deserialized value was found to be valid.
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    ///     The collection of validation errors.
    /// </summary>
#pragma warning disable CS8618
    public IList<ValidationFailure> Errors { get; set; }
#pragma warning restore CS8618

    public string ErrorMessage() =>
#pragma warning disable CS8603
        Errors
            .Select(error => error.ToString())
            .ToString();
#pragma warning restore CS8603
}