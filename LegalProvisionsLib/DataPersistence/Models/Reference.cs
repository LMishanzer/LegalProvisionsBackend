using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class Reference
{
    [BsonElement(elementName: "provision_id")]
    public Guid ProvisionId { get; set; }
    
    [BsonElement(elementName: "content_item_id")]
    public Guid ContentItemId { get; set; }
}