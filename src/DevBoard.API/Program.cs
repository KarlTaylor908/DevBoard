using DevBoard.API.Infrastructure.Data;
using DevBoard.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("develop");


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<AuthService>();

builder.Services.Configure<LockoutOptions>(
    builder.Configuration.GetSection("Auth:Lockout"));
builder.Services.AddOptions<LockoutOptions>()
    .Bind(builder.Configuration.GetSection("Auth:Lockout"))
    .ValidateDataAnnotations();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
