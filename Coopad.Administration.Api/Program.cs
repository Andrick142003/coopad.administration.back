using Coopad.Administration.Api.Configuration;
using Coopad.Administration.Api.Infrastructure.Database;
using Coopad.Administration.Api.Middlewares;
using Coopad.Administration.Api.Repositories;
using Coopad.Administration.Api.Repositories.Interfaces;
using Coopad.Administration.Api.Services;
using Coopad.Administration.Api.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AseConnectionSettings>(
    builder.Configuration.GetSection("AseConnection"));


builder.Services.AddSingleton<IAseConnectionFactory, AseConnectionFactory>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IHealthRepository, HealthRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

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
