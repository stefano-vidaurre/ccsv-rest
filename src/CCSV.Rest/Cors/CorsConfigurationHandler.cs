using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CCSV.Rest.Cors;

public static class CorsConfigurationHandler
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        CorsConfiguration? corsConfiguration = configuration.GetSection("Cors").Get<CorsConfiguration>();
        CorsPolicyBuilder policyBuilder = new CorsPolicyBuilder();

        if (corsConfiguration is null)
        {
            services.AddCors();
            return services;
        }

        policyBuilder.WithOrigins(corsConfiguration.AllowedOrigins ?? Array.Empty<string>());

        if (corsConfiguration.AllowedHeaders is not null)
        {
            policyBuilder.WithHeaders(corsConfiguration.AllowedHeaders);
        }
        else
        {
            policyBuilder.AllowAnyHeader();
        }

        if (corsConfiguration.AllowedMethods is not null)
        {
            policyBuilder.WithMethods(corsConfiguration.AllowedMethods);
        }
        else
        {
            policyBuilder.AllowAnyMethod();
        }

        if (corsConfiguration.AllowedCredentials)
        {
            policyBuilder.AllowCredentials();
        }
        else
        {
            policyBuilder.DisallowCredentials();
        }

        services.AddCors(options => options.AddDefaultPolicy(policyBuilder.Build()));

        return services;
    }
}
