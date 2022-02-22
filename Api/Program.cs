using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Profiles;
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
                services.AddDbContext<ChangeContext>(options => DbParams(options));
                services.AddDbContext<ImageContext>(options => DbParams(options));
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

                static DbContextOptionsBuilder DbParams(DbContextOptionsBuilder options)
                {
                    return options.UseCosmos(
                        Environment.GetEnvironmentVariable("AccountEndpoint")!,
                        Environment.GetEnvironmentVariable("AccountKey")!,
                        Environment.GetEnvironmentVariable("DatabaseName")!);
                }
            })
            .Build();

        host.Run();
    }
}
