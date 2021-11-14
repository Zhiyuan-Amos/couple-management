using System;
using Couple.Api;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Api.Profiles;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Couple.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<ChangeContext>(options => DbParams(options));
            builder.Services.AddAutoMapper(typeof(ChangeProfile), typeof(ImageProfile));

            if (builder.GetContext().EnvironmentName == "Development")
            {
                builder.Services.AddScoped<ICurrentUserService, DevelopmentCurrentUserService>();
            }
            else
            {
                builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            }
            builder.Services.AddSingleton<IDateTimeService, DateTimeService>();

            static DbContextOptionsBuilder DbParams(DbContextOptionsBuilder options) => options.UseCosmos(
                Environment.GetEnvironmentVariable("AccountEndpoint")!,
                Environment.GetEnvironmentVariable("AccountKey")!,
                Environment.GetEnvironmentVariable("DatabaseName")!);
        }
    }
}
