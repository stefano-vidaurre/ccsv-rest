using CCSV.Rest.Cors;
using CCSV.Rest.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CCSV.Rest;

public static class CCSVRestExtensions
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration) {
        return CorsConfigurationHandler.Configure(services, configuration);
    }

    public static IApplicationBuilder UseHttpStatusCodeExceptionHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<HttpStatusCodeExceptionHandler>();

        return builder;
    }
}
