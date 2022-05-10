using System.Text.Json;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker.Http;

namespace Couple.Api.Infrastructure;

// Adapted from https://www.tomfaltesek.com/azure-functions-input-validation/
public static class HttpRequestDataExtensions
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
    public static async Task<ValidatableRequest<T>> GetJsonBody<T, TV>(this HttpRequestData request)
        where TV : AbstractValidator<T>, new()
    {
        var requestBody = await request.ReadAsStringAsync();
#pragma warning disable CS8604
        var requestObject = JsonSerializer.Deserialize<T>(requestBody, Options);
#pragma warning restore CS8604
        var validator = new TV();
#pragma warning disable CS8604
        var validationResult = await validator.ValidateAsync(requestObject);
#pragma warning restore CS8604

        if (!validationResult.IsValid)
        {
            return new()
            {
                Value = requestObject,
                Json = requestBody,
                IsValid = false,
                Errors = validationResult.Errors
            };
        }

        return new() { Value = requestObject, Json = requestBody, IsValid = true };
    }
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
        string.Join('\n', Errors
            .Select(error => error.ToString())
            .ToList());
}
