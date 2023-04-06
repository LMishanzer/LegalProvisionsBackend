using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class ContentItem
{
    [BsonElement(elementName: "id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [BsonElement(elementName: "title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement(elementName: "text_main")]
    public string TextMain { get; set; } = string.Empty;
    
    [BsonElement(elementName: "inner_items")]
    public IEnumerable<ContentItem> InnerItems { get; set; } = Array.Empty<ContentItem>();

    [BsonElement(elementName: "inner_items_type")]
    public string InnerItemsType { get; set; } = string.Empty;

    [BsonElement(elementName: "text_elements")]
    public IEnumerable<TextElement> TextElements { get; set; } = Array.Empty<TextElement>();
}