using Couple.Messaging;
using Couple.Messaging.Data;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Couple.Messaging;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddDbContext<ImageContext>(options => DbParams(options));

        static DbContextOptionsBuilder DbParams(DbContextOptionsBuilder options)
        {
            return options.UseCosmos(Environment.GetEnvironmentVariable("DatabaseName")!,
                Environment.GetEnvironmentVariable("DatabaseConnectionString")!);
        }
    }
}
