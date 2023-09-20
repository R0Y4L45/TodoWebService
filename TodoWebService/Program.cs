using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using TodoWebService;
using TodoWebService.BackgoundServices;
using TodoWebService.Data;
using TodoWebService.Models.DTOs.Validations;
using TodoWebService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddLogging(c => c.AddJsonConsole());

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.Console()
//    .CreateLogger();

//var outputTemplate = "\"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine} Environment:{Environment} ThreadId: {ThreadId} {Exception}\"";

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//    .MinimumLevel.Debug()
//    .Enrich.WithThreadId()
//    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
//    .WriteTo.Console(outputTemplate: outputTemplate)
//    .WriteTo.File(@"C:\Users\namiqrasullu\source\repos\TodoWebService\TodoWebService\logs\mylog.txt", rollingInterval: RollingInterval.Day, outputTemplate : outputTemplate)
//    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("TodoDbConnectionString"), "logs", autoCreateSqlTable: true)
//    .CreateLogger();

//builder.Host.UseSerilog();

//builder.Services.AddOutputCache();

builder.Services.AddMemoryCache(); // singleton


builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<TodoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoDbConnectionString"));
});

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddDomainServices();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
builder.Services.AddHostedService(s => new EmailBackgroundService(s));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseOutputCache();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
