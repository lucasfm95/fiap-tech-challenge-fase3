using Fiap.TechChallenge.Infrastructure.Context;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Fiap.TechChallenge.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("contactsCrud")
        .WithPassword("123456")
        .Build();
    
    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithUsername("admin")
        .WithPassword("admin")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("RUN_MIGRATIONS", "true", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTests", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("MASS_TRANSIT_INSERT_QUEUE_NAME", "contact-insert-queue");
        Environment.SetEnvironmentVariable("MASS_TRANSIT_UPDATE_QUEUE_NAME", "contact-update-queue");
        Environment.SetEnvironmentVariable("MASS_TRANSIT_DELETE_QUEUE_NAME", "contact-delete-queue");
        
        builder.ConfigureServices(services => 
        {
            var connectionString = $"{_postgreSqlContainer.GetConnectionString()};Include Error Detail=true;";
            
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ContactDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            
            services.AddDbContext<ContactDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            
            services.AddHealthChecks()
                .AddNpgSql(connectionString);
        });
        
        builder.ConfigureTestServices(services => services.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(_rabbitMqContainer.GetConnectionString());
                cfg.ConfigureEndpoints(context);
            });
        }));
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _rabbitMqContainer.StopAsync();
        await _postgreSqlContainer.StopAsync();
    }
}