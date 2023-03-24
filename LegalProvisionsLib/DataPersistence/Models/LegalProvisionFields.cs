using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class LegalProvisionFields
{
    [BsonElement(elementName: "key_words")]
    public IEnumerable<string>? KeyWords { get; set; }

    [BsonElement(elementName: "issue_time")]
    public DateTime? IssueTime { get; set; }

    [BsonElement(elementName: "content")]
    public ContentItem? Content { get; set; }
}