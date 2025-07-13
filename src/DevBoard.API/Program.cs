using DevBoard.Infrastructure;
using DevBoard.Infrastructure.Auth;
using DevBoard.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("develop");


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<LockoutOptions>(
    builder.Configuration.GetSection("Auth:Lockout"));
builder.Services.AddOptions<LockoutOptions>()
    .Bind(builder.Configuration.GetSection("Auth:Lockout"))
    .ValidateDataAnnotations();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings is null)
    throw new InvalidOperationException("No JwtSetting configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };

        // Enable reading JWT from cookie
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("jwt_token"))
                {
                    context.Token = context.Request.Cookies["jwt_token"];
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

var lockoutOptions = app.Services.GetRequiredService<IOptions<LockoutOptions>>().Value;
if (lockoutOptions.MaxFailedAccessAttempts <= 0 || lockoutOptions.DefaultLockoutTimeSpan.TotalMilliseconds <= 0)
{
    throw new InvalidOperationException("Invalid Lockout policy configuration.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

