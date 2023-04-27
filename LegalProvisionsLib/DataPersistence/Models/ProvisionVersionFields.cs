using MongoDB.Bson.Serialization.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LegalProvisionsLib.DataPersistence.Models;

public class ProvisionVersionFields
{
    [BsonElement(elementName: "provision_header")]
    public Guid ProvisionHeader { get; set; }

    [BsonElement(elementName: "issue_date")]
    public DateOnly IssueDate { get; set; }

    [BsonElement(elementName: "valid_from")]
    public DateOnly? ValidFrom { get; set; }

    [BsonElement(elementName: "takes_effect_from")]
    public DateOnly? TakesEffectFrom { get; set; }
    
    [BsonElement(elementName: "document_name")]
    public FileMetadata? FileMetadata { get; set; }

    [BsonElement(elementName: "content")]
    public ContentItem? Content { get; set; }

    public IEnumerable<ContentItem> GetAllContentDictionary()
    {
        return Content?.GetAllContentInArray() ?? Array.Empty<ContentItem>();
    }
}