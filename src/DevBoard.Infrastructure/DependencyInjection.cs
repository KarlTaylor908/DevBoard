using DevBoard.Application.Auth;
using DevBoard.Application.Auth.Interfaces;
using DevBoard.Infrastructure.Auth;
using DevBoard.Infrastructure.Auth.Services;
using DevBoard.infrastructure.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevBoard.Application.User.Interfaces;
using DevBoard.Infrastructure.User.Services;
using DevBoard.Application.Tickets.Interfaces;
using DevBoard.Infrastructure.Tickets.Services;
using DevBoard.Domain.Tickets;
using DevBoard.Infrastructure.Tickets;
using DevBoard.Domain.Boards;
using DevBoard.Application.Boards.Interfaces;
using DevBoard.Infrastructure.Boards;
using DevBoard.Infrastructure.Boards.Services;

namespace DevBoard.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            services.AddScoped<IBoardService, BoardService>();
            services.AddScoped<IBoardRepository, BoardRepository>();
          
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
