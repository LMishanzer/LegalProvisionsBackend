using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.Settings;

namespace LegalProvisionsBackend;

public class Startup
{
    public IConfigurationRoot Configuration { get; }
    
    public Startup(IConfigurationRoot configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        var settings = new SettingsReader().ReadSettings($"Settings{Path.PathSeparator}server.settings.json");
        MongoSettings.Instance = settings.MongoSettings;

        services.AddTransient<IDataPersistence, MongoPersistence>();
        services.AddControllers();
    }
    
    public void Configure(IApplicationBuilder app)
    {
        
    }
}