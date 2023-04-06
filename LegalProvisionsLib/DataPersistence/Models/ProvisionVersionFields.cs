using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class ProvisionVersionFields
{
    [BsonElement(elementName: "provision_header")]
    public Guid ProvisionHeader { get; set; }

    [BsonElement(elementName: "issue_time")]
    public DateTime? IssueTime { get; set; }

    public DateTime? ValidFrom { get; set; }

    [BsonElement("takes_effect_from")]
    public DateTime? TakesEffectFrom { get; set; }

    [BsonElement(elementName: "content")]
    public ContentItem? Content { get; set; }
}