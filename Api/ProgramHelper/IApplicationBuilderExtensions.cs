using System.Net.Mime;

namespace Couple.Api.ProgramHelper;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-6.0#exception-handler-lambda
        builder.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                return Task.CompletedTask;
            });
        });

        return builder;
    }
}
