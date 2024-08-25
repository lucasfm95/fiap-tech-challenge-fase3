using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Fiap.TechChallenge.Api.Configurations;

internal static class SwaggerConfig
{
    internal static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Fiap TechChallenge phase 2", Version = "v1" });
            
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }
}