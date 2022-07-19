namespace Couple.Api.ProgramHelper;

public static class IMvcBuilderExtensions
{
    public static IMvcBuilder LogInvalidModelState(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options =>
        {
            // https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0#log-automatic-400-responses

            // To preserve the default behavior, capture the original delegate to call later.
            var builtInFactory = options.InvalidModelStateResponseFactory;

            options.InvalidModelStateResponseFactory = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();

                var errorMessages = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage);
                var toLog = string.Join('\n', errorMessages);
                logger.LogWarning("{ModelStateErrors}", toLog);

                return builtInFactory(context);
            };
        });
        return builder;
    }
}
