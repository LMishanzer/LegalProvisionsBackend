using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class ProvisionHeaderFields
{
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement(elementName: "keywords")]
    public IEnumerable<string> Keywords { get; set; } = Array.Empty<string>();
}