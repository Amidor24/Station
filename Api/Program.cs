using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StationDBContext") ?? throw new InvalidOperationException("Connection string 'StationDBContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
