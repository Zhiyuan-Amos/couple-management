using Couple.Api.ProgramHelper;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace Couple.Api;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(services =>
            {
                services.AddHttpClient("Image")
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)));
                services.AddDbContext<ChangeContext>(DbParams);
                services.AddDbContext<ImageContext>(DbParams);
                services.AddAutoMapper(typeof(ChangeProfile));

                var environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
                if (environmentName == "Development")
                {
                    services.AddScoped<ICurrentUserService, DevelopmentCurrentUserService>();
                }
                else
                {
                    services.AddScoped<ICurrentUserService, CurrentUserService>();
                }

                services.AddSingleton<IDateTimeService, DateTimeService>();

                static void DbParams(DbContextOptionsBuilder options)
                {
                    options.UseCosmos(
                        Environment.GetEnvironmentVariable("AccountEndpoint")!,
                        Environment.GetEnvironmentVariable("AccountKey")!,
                        Environment.GetEnvironmentVariable("DatabaseName")!);
                }
            })
            .Build();

        host.Run();
    }
}
