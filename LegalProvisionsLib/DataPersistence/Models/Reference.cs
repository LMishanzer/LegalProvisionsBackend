using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class Reference
{
    [BsonElement(elementName: "provision_id")]
    public Guid ProvisionId { get; set; }
}