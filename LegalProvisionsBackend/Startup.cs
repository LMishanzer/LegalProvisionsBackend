using LegalProvisionsBackend.Middleware;
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
        var settings = new SettingsReader().ReadSettings($"Settings\\server.settings.json");

        services.AddSingleton<MongoSettings>(_ => settings.MongoSettings);
        services.AddTransient<IDataPersistence, MongoPersistence>();
        services.AddControllers();
    }
    
    public void Configure(WebApplication app)
    {
        app.UseMiddleware<ExceptionHandler>();
        app.MapControllers();
    }
}