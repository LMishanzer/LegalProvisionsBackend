using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class Article
{
    [BsonElement(elementName: "title")]
    public string Title { get; set; }

    [BsonElement(elementName: "paragraphs")]
    public IEnumerable<string> Paragraphs { get; set; }
}