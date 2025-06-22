using DevBoard.Application.Auth;
using DevBoard.Application.Auth.Interfaces;
using DevBoard.Infrastructure.Auth;
using DevBoard.Infrastructure.Auth.Services;
using DevBoard.infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevBoard.Application.Tickets.Interfaces;
using DevBoard.Infrastructure.Tickets.Services;
using DevBoard.Domain.Tickets;
using DevBoard.Infrastructure.Tickets;

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

            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            return services;
        }
    }
}
