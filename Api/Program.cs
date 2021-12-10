using System;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Couple.Api;

public class Program
{
    public static void Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(builder =>
            {
                builder.AddHttpClient();
                builder.AddDbContext<ChangeContext>(options => DbParams(options));
                builder.AddAutoMapper(typeof(ChangeProfile), typeof(ImageProfile));

                var environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
                if (environmentName == "Development")
                {
                    builder.AddScoped<ICurrentUserService, DevelopmentCurrentUserService>();
                }
                else
                {
                    builder.AddScoped<ICurrentUserService, CurrentUserService>();
                }
                builder.AddSingleton<IDateTimeService, DateTimeService>();

                static DbContextOptionsBuilder DbParams(DbContextOptionsBuilder options) => options.UseCosmos(
                    Environment.GetEnvironmentVariable("AccountEndpoint")!,
                    Environment.GetEnvironmentVariable("AccountKey")!,
                    Environment.GetEnvironmentVariable("DatabaseName")!);
            })
            .Build();

        host.Run();
    }
}
