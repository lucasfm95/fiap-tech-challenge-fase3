using System.Text.Json.Serialization;
using Fiap.TechChallenge.Api.Configurations;
using Fiap.TechChallenge.Api.Middlewares;
using Fiap.TechChallenge.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (env != "IntegrationTests")
{
    builder.Services.AddHealthChecks()
        .AddNpgSql(Environment.GetEnvironmentVariable("CONNECTION_STRING_DB_POSTGRES") ?? 
                   throw new Exception("CONNECTION_STRING_DB_POSTGRES not found."));
}

builder.Services.AddDbContext<ContactDbContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("CONNECTION_STRING_DB_POSTGRES"));
});

builder.Services.RegisterApplicationServices();
builder.Services.RegisterRepositories();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var counter = Metrics.CreateCounter(
    "fiap_tech_challenge_api_request_counter", "Contador de requisições HTTP", new CounterConfiguration
{
    LabelNames = ["method", "endpoint"]
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHealthcheck();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.Use((context, next) =>
{
    counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
    return next();
});
app.UseMetricServer(settings => settings.EnableOpenMetrics = false);
app.UseHttpMetrics();

if (env == "IntegrationTests")
{
    bool.TryParse(Environment.GetEnvironmentVariable("RUN_MIGRATIONS"), out var runMigrations);
    if (runMigrations)
    {
        await RunMigration();
    }   
}

app.Run();
return;

async Task RunMigration()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
    await dbContext.Database.MigrateAsync();
}
public partial class Program
{
}