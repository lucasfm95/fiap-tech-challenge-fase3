using Fiap.TechChallenge.Application.Repositories;
using Fiap.TechChallenge.Application.Services;
using Fiap.TechChallenge.Application.Services.Interfaces;
using Fiap.TechChallenge.Infrastructure.Repositories;

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
    }
}