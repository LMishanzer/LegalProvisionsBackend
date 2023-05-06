using LegalProvisionsBackend.Middleware;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.Differences;
using LegalProvisionsLib.Differences.DifferenceCalculator;
using LegalProvisionsLib.Documents;
using LegalProvisionsLib.FileStorage;
using LegalProvisionsLib.Logging;
using LegalProvisionsLib.Logging.Persistence;
using LegalProvisionsLib.Logging.Persistence.Elastic;
using LegalProvisionsLib.ProvisionWarehouse.DataHandling.Header;
using LegalProvisionsLib.ProvisionWarehouse.DataHandling.Version;
using LegalProvisionsLib.ProvisionWarehouse.Header;
using LegalProvisionsLib.ProvisionWarehouse.Version;
using LegalProvisionsLib.Search;
using LegalProvisionsLib.Search.HeaderIndexing;
using LegalProvisionsLib.Search.Indexing.Fulltext;
using LegalProvisionsLib.Search.Indexing.Keywords;
using LegalProvisionsLib.Search.SearchResultHandling;
using LegalProvisionsLib.Search.VersionIndexing;
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
                policy =>
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
        services.AddSingleton<IDifferenceCalculator, DifferenceCalculator>();
        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<ILogPersistence, ElasticsearchPersistence>();
        services.AddSingleton<IFileStorage, FilesystemStorage>();
        services.AddSingleton<IKeywordsIndexer, KeywordsIndexer>();
        services.AddSingleton<IFulltextIndexer, FulltextIndexer>();
        
        services.AddTransient<IVersionHandler, VersionHandler>();
        services.AddTransient<IHeaderHandler, HeaderHandler>();
        services.AddTransient<IVersionWarehouse, VersionWarehouse>();
        services.AddTransient<IHeaderWarehouse, HeaderWarehouse>();
        services.AddTransient<IVersionIndexManager, VersionIndexManager>();
        services.AddTransient<IHeaderIndexManager, HeaderIndexManager>();
        services.AddTransient<ISearchHandler, SearchHandler>();
        services.AddTransient<IDocumentManager, DocumentManager>();
        services.AddTransient<IDifferenceManager, DifferenceManager>();
        services.AddTransient<ISearchResultHandler, SearchResultHandler>();
    }
}