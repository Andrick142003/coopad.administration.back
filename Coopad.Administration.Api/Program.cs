using Coopad.Administration.Api.Configuration;
using Coopad.Administration.Api.Data;
using Coopad.Administration.Api.Infrastructure.Database;
using Coopad.Administration.Api.Middlewares;
using Coopad.Administration.Api.Repositories;
using Coopad.Administration.Api.Repositories.Interfaces;
using Coopad.Administration.Api.Services;
using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AseConnectionSettings>(
    builder.Configuration.GetSection("AseConnection"));

builder.Services.Configure<ActiveDirectorySettings>(
    builder.Configuration.GetSection("ActiveDirectory"));


builder.Services.AddSingleton<IAseConnectionFactory, AseConnectionFactory>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IHealthRepository, HealthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SecurityDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SecurityDatabase")
    )
);

var app = builder.Build();


app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

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
