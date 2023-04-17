namespace LegalProvisionsLib.Settings;

public class ServerSettings
{
    public MongoSettings MongoSettings { get; set; } = new();
    public ElasticSettings ElasticSettings { get; set; } = new();
}