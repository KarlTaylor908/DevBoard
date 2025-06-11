using DevBoard.Application.Auth;
using DevBoard.Application.Auth.Interfaces;
using DevBoard.Infrastructure.Auth;
using DevBoard.Infrastructure.Auth.Services;
using DevBoard.infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevBoard.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, AuthRepository>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}
