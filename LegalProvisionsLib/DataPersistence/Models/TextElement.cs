using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class TextElement
{
    [BsonElement(elementName: "start_index")]
    public int StartIndex { get; set; }
    
    [BsonElement(elementName: "end_index")]
    public int EndIndex { get; set; }

    [BsonElement(elementName: "element_type")]
    public TextElementType ElementType { get; set; }
    
    [BsonElement(elementName: "comment")]
    public string? Comment { get; set; }
    
    [BsonElement(elementName: "reference")]
    public Reference? Reference { get; set; }
}

public enum TextElementType
{
    Reference,
    Comment
}