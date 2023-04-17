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
        
        app.UseMiddleware<ExceptionHandler>();
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        
        app.UseRouting();
        app.UseCors(builder => builder.WithOrigins("http://localhost:4200"));
        app.UseAuthorization();
        app.MapControllers();
    }
}