using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class DataItem<T> where T: class
{
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonElement(elementName: "creation_time")]
    public DateTime CreationTime { get; set; }

    [BsonElement(elementName: "fields")]
    public T Fields { get; set; }
    
    public DataItem(T fields)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.Now;
        Fields = fields;
    }
}