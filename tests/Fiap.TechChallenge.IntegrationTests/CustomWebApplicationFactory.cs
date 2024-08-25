using Fiap.TechChallenge.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Fiap.TechChallenge.IntegrationTests;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("contactsCrud")
        .WithPassword("123456")
        .Build();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("RUN_MIGRATIONS", "true", EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTests", EnvironmentVariableTarget.Process);
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
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _postgreSqlContainer.StopAsync();
    }
}