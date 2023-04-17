using LegalProvisionsBackend.Middleware;
using LegalProvisionsLib.DataHandling;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.Differences;
using LegalProvisionsLib.Settings;
using MongoDB.Driver;

namespace LegalProvisionsBackend;

public class Startup
{
    private const string CorsPolicy = "AllowSpecificOrigins";

    public void ConfigureServices(IServiceCollection services)
    {
        var settings = new SettingsReader().ReadSettings($"Settings\\server.settings.json");
        var client = new MongoClient(settings.MongoSettings.ConnectionUri);
        var database = client.GetDatabase(settings.MongoSettings.DatabaseName);

        services.AddSingleton<MongoSettings>(_ => settings.MongoSettings);
        services.AddTransient<ProvisionVersionPersistence>();
        services.AddTransient<ProvisionHeaderPersistence>();
        services.AddTransient<IDifferenceCalculator, DifferenceCalculator>();
        services.AddTransient<IProvisionHandler, ProvisionHandler>();
        services.AddSingleton<IMongoDatabase>(_ => database);
        
        services.AddControllers();
        services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicy,
                policy  =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                });
        });
    }
    
    public void Configure(WebApplication app)
    {
        app.UseMiddleware<ExceptionHandler>();
        app.UseCors(CorsPolicy);
        app.MapControllers();
    }
}