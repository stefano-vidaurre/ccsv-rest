using CCSV.Rest.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace CCSV.Rest;

public static class CCSVRestExtensions
{
    public static IApplicationBuilder UseHttpStatusCodeExceptionHandler(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<HttpStatusCodeExceptionHandler>();

        return builder;
    }
}
