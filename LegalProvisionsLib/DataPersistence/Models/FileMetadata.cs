using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class FileMetadata
{
    [BsonElement(elementName: "name")]
    public string Name { get; set; }
    
    [BsonElement(elementName: "name_in_storage")]
    public string NameInStorage { get; set; }
    
    public FileMetadata(string name, string nameInStorage)
    {
        Name = name;
        NameInStorage = nameInStorage;
    }
}