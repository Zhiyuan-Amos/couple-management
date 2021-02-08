using Api.Data;
using Api.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Api.Startup))]

namespace Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<EventContext>(options => dbParams(options));
            builder.Services.AddDbContext<UserContext>(options => dbParams(options));

            if (builder.GetContext().EnvironmentName == "Development")
            {
                builder.Services.AddScoped<ICurrentUserService, DevelopmentCurrentUserService>();
            }
            else
            {
                builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            }
            builder.Services.AddSingleton<IDateTimeService, DateTimeService>();

            static DbContextOptionsBuilder dbParams(DbContextOptionsBuilder options) => options.UseCosmos(
                Environment.GetEnvironmentVariable("AccountEndpoint"),
                Environment.GetEnvironmentVariable("AccountKey"),
                Environment.GetEnvironmentVariable("DatabaseName"));
        }
    }
}
