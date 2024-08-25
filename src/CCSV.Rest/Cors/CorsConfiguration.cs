namespace CCSV.Rest.Cors;

public class CorsConfiguration
{
    public string[]? AllowedOrigins { get; init; }
    public string[]? AllowedHeaders { get; init; }
    public string[]? AllowedMethods { get; init; }
    public bool AllowedCredentials { get; init; } = false;
}
