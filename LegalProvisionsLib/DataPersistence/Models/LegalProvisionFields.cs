using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class LegalProvisionFields
{
    [BsonElement(elementName: "title")]
    public string? Title { get; set; }
    
    [BsonElement(elementName: "key_words")]
    public IEnumerable<string>? KeyWords { get; set; }

    [BsonElement(elementName: "issue_time")]
    public DateTime? IssueTime { get; set; }

    public DateTime? ValidFrom { get; set; }

    [BsonElement("takes_effect_from")]
    public DateTime? TakesEffectFrom { get; set; }

    [BsonElement(elementName: "content")]
    public ContentItem? Content { get; set; }
}