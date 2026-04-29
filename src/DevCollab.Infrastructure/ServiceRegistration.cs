using DevCollab.Application.Interfaces;
using DevCollab.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevCollab.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtGenerator, JwtGeneratorService>();
        services.AddScoped<IPasswordHasher, PasswordHasherService>();
        services.AddTransient<IEmailSender, EmailSenderService>();
        services.AddSingleton<IMediaStorageService, MinioMediaStorageService>();

        return services;
    }
}
