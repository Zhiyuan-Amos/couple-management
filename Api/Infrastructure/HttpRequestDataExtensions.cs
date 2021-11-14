using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker.Http;

namespace Couple.Api.Infrastructure
{
    // Adapted from https://www.tomfaltesek.com/azure-functions-input-validation/
    public static class HttpRequestDataExtensions
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        /// <summary>
        /// Returns the deserialized request body with validation information.
        /// </summary>
        /// <typeparam name="T">Type used for deserialization of the request body.</typeparam>
        /// <typeparam name="TV">
        /// Validator used to validate the deserialized request body.
        /// </typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<ValidatableRequest<T>> GetJsonBody<T, TV>(this HttpRequestData request)
            where TV : AbstractValidator<T>, new()
        {
            var requestBody = await request.ReadAsStringAsync();
            var requestObject = JsonSerializer.Deserialize<T>(requestBody, Options);
            var validator = new TV();
            var validationResult = validator.Validate(requestObject);

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

            return new()
            {
                Value = requestObject,
                Json = requestBody,
                IsValid = true
            };
        }
    }

    public class ValidatableRequest<T>
    {
        /// <summary>
        /// The deserialized value of the request.
        /// </summary>
        public T Value { get; set; }

        public string Json { get; set; }

        /// <summary>
        /// Whether or not the deserialized value was found to be valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// The collection of validation errors.
        /// </summary>
        public IList<ValidationFailure> Errors { get; set; }

        public string ErrorMessage()
        {
            return string.Join('\n', Errors
                .Select(error => error.ToString())
                .ToList());
        }
    }
}
