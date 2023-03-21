using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class LegalProvision
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement(elementName: "title")]
    public string Title { get; set; }

    [BsonElement(elementName: "articles")]
    public IEnumerable<Article> Articles { get; set; }
}