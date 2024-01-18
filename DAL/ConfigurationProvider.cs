using Microsoft.Extensions.Configuration;

public class ConfigurationProvider
{
    public IConfiguration Configuration { get; }

    public ConfigurationProvider()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        Configuration = builder.Build();
    }
    public string GetConnectionString()
    {
        return Configuration.GetConnectionString("DefaultConnection");
    }
}
