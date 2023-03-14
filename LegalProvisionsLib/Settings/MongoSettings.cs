namespace LegalProvisionsLib.Settings;

public class MongoSettings
{
    public static MongoSettings Instance;
    
    public string ConnectionUri { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}