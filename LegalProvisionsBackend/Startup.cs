using LegalProvisionsBackend.Middleware;
using LegalProvisionsLib.DataHandling;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.Differences;
using LegalProvisionsLib.Search;
using LegalProvisionsLib.Search.Indexing;
using LegalProvisionsLib.Settings;
using MongoDB.Driver;

namespace LegalProvisionsBackend;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var settings = new SettingsReader().ReadSettings($"Settings{Path.DirectorySeparatorChar}server.settings.json");
        var client = new MongoClient(settings.MongoSettings.ConnectionUri);
        var database = client.GetDatabase(settings.MongoSettings.DatabaseName);

        services.AddTransient<MongoSettings>(_ => settings.MongoSettings);
        services.AddTransient<ElasticSettings>(_ => settings.ElasticSettings);
        services.AddTransient<ProvisionVersionPersistence>();
        services.AddTransient<ProvisionHeaderPersistence>();
        services.AddTransient<IDifferenceCalculator, DifferenceCalculator>();
        services.AddTransient<IProvisionHandler, ProvisionHandler>();
        services.AddTransient<IMongoDatabase>(_ => database);
        services.AddTransient<IIndexer, ElasticsearchIndexer>();
        services.AddTransient<ISearchHandler, SearchHandler>();
        
        services.AddControllers();
    }
    
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseFileServer();
        
        app.UseMiddleware<ExceptionHandler>();

        app.UseRouting();
        app.UseCors(builder => builder.WithOrigins("http://localhost:4200", "http://95.179.243.24"));
        app.UseAuthorization();
        app.MapControllers();

        app.MapGet("/{**catchall}", async context =>
        {
            context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            context.Response.Headers.Add("Pragma", "no-cache");
            context.Response.Headers.Add("Expires", "0");

            await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
        });
    }
}