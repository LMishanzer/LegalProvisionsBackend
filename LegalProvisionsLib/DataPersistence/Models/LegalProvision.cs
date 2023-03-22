using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class LegalProvision
{
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement(elementName: "creation_time")]
    public DateTime CreationTime { get; set; }

    [BsonElement(elementName: "fields")]
    public LegalProvisionFields Fields { get; set; }

    public LegalProvision(LegalProvisionFields fields)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.Now;
        Fields = fields;
    }
}