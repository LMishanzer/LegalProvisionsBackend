using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class LegalProvision
{
    [BsonId]
    public string Id { get; set; }
    
    public string Title { get; set; }

    public IEnumerable<Article> Articles { get; set; }
}