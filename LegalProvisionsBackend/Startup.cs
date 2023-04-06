﻿using LegalProvisionsBackend.Middleware;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.Settings;

namespace LegalProvisionsBackend;

public class Startup
{
    private const string CorsPolicy = "AllowSpecificOrigins";
    
    public IConfigurationRoot Configuration { get; }
    
    public Startup(IConfigurationRoot configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        var settings = new SettingsReader().ReadSettings($"Settings\\server.settings.json");

        services.AddSingleton<MongoSettings>(_ => settings.MongoSettings);
        services.AddTransient<MongoPersistence>();
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
        app.MapControllers();
        app.UseCors(CorsPolicy);
    }
}