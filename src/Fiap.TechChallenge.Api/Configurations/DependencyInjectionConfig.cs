using Fiap.TechChallenge.Application.MessageBroker;
using Fiap.TechChallenge.Application.Repositories;
using Fiap.TechChallenge.Application.Services;
using Fiap.TechChallenge.Application.Services.Interfaces;
using Fiap.TechChallenge.Infrastructure.MessageBroker;
using Fiap.TechChallenge.Infrastructure.Repositories;
using MassTransit;

namespace Fiap.TechChallenge.Api.Configurations;

internal static class DependencyInjectionConfig
{
    internal static void RegisterRepositories(this IServiceCollection serviceProvider)
    {
        serviceProvider.AddScoped<IContactRepository, ContactRepository>();
    }
    
    internal static void RegisterApplicationServices(this IServiceCollection serviceProvider)
    {
        serviceProvider.AddScoped<IContactService,ContactService>();
        serviceProvider.AddScoped<IPublisherService, PublisherService>();
    }
    
    internal static void RegisterMessageBroker(this IServiceCollection serviceProvider)
    {
        serviceProvider.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.UsingRabbitMq((context, cfg) =>
            {
                var massTransitHost = Environment.GetEnvironmentVariable("MASS_TRANSIT_HOST") ?? string.Empty;
                var massTransitPort = ushort.Parse(Environment.GetEnvironmentVariable("MASS_TRANSIT_PORT") ?? "0");
                var massTransitUser = Environment.GetEnvironmentVariable("MASS_TRANSIT_USERNAME") ?? string.Empty;
                var massTransitPassword = Environment.GetEnvironmentVariable("MASS_TRANSIT_PASSWORD") ?? string.Empty;
        
                cfg.Host(massTransitHost, massTransitPort, "/", hostConfigurator =>
                {
                    hostConfigurator.Username(massTransitUser);
                    hostConfigurator.Password(massTransitPassword);
                });
        
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}