using LegalProvisionsBackend.Middleware;
using LegalProvisionsLib.DataHandling;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.Differences;
using LegalProvisionsLib.Documents;
using LegalProvisionsLib.FileStorage;
using LegalProvisionsLib.Logging;
using LegalProvisionsLib.Logging.Persistence;
using LegalProvisionsLib.Logging.Persistence.Elastic;
using LegalProvisionsLib.Search;
using LegalProvisionsLib.Search.Indexing.FulltextIndexing;
using LegalProvisionsLib.Search.Indexing.KeywordsIndexing;
using LegalProvisionsLib.Settings;
using MongoDB.Driver;
using ILogger = LegalProvisionsLib.Logging.ILogger;

namespace LegalProvisionsBackend;

public class Startup
{
    private const string CorsPolicy = "AllowSpecificOrigins";
    
    public void ConfigureServices(IServiceCollection services)
    {
        RegisterDependencies(services);
        
        services.AddControllers();
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicy,
                policy  =>
                {
                    policy.WithOrigins("*")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }
    
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseFileServer();
        
        app.UseMiddleware<RequestMiddleware>();
        app.UseMiddleware<ExceptionHandler>();

        app.UseRouting();
        app.UseCors(CorsPolicy);
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action}");

        app.MapFallbackToFile("index.html");
    }

    private static void RegisterDependencies(IServiceCollection services)
    {
        var settingsPath = Path.Combine("Settings", "server.settings.json");
        var settings = new SettingsReader().ReadSettings(settingsPath);
        var client = new MongoClient(settings.MongoSettings.ConnectionUri);
        var database = client.GetDatabase(settings.MongoSettings.DatabaseName);

        services.AddSingleton<MongoSettings>(_ => settings.MongoSettings);
        services.AddSingleton<ElasticSettings>(_ => settings.ElasticSettings);
        services.AddSingleton<IMongoDatabase>(_ => database);
        services.AddSingleton<ProvisionVersionPersistence>();
        services.AddSingleton<ProvisionHeaderPersistence>();
        
        services.AddTransient<IDifferenceCalculator, DifferenceCalculator>();
        services.AddTransient<IProvisionHandler, ProvisionHandler>();
        services.AddTransient<IKeywordsIndexer, KeywordsIndexer>();
        services.AddTransient<IFulltextIndexer, FulltextIndexer>();
        services.AddTransient<ISearchHandler, SearchHandler>();
        services.AddTransient<IFileStorage, FilesystemStorage>();
        services.AddTransient<IDocumentManager, DocumentManager>();
        services.AddTransient<ILogger, Logger>();
        services.AddTransient<ILogPersistence, ElasticsearchPersistence>();
    }
}