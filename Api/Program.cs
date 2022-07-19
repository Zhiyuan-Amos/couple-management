using System.Reflection;
using Couple.Api.ProgramHelper;
using Couple.Api.Shared.Data;
using Couple.Api.Shared.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Couple.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services
            .AddControllers(options => options.UseNamespaceRouteToken())
            .LogInvalidModelState()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services
            .AddHttpClient("Image")
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)));

        builder.Services
            .AddCors(builder.Environment);

        if (!builder.Environment.IsDevelopment())
        {
            builder.Services
                .AddB2CAuthentication(builder.Configuration)
                .AddDefaultAuthorization();
        }

        builder.Services
            .AddHealthChecks();
        
        builder.Services
            .AddDbContext<ChangeContext>(DbParams)
            .AddDbContext<ImageContext>(DbParams)
            .AddAutoMapper(typeof(ChangeProfile))
            .AddSingleton<IDateTimeService, DateTimeService>()
            .AddUserService(builder.Environment);

        var app = builder.Build();

        app.UseCustomExceptionHandler()
            .UseRouting()
            .UseCors()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints.MapControllers());

        app.MapHealthChecks("/health")
            .AllowAnonymous();

        app.Run();

        void DbParams(DbContextOptionsBuilder options)
        {
            options.UseCosmos(
                builder.Configuration.GetValue<string>("Database:Endpoint")!,
                builder.Configuration.GetValue<string>("Database:Key")!,
                builder.Configuration.GetValue<string>("Database:Name")!);
        }
    }
}
