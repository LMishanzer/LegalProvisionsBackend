using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class ContentItem
{
    [BsonElement(elementName: "id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonElement(elementName: "identifier")]
    public string Identifier { get; set; } = string.Empty;
    
    [BsonElement(elementName: "title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement(elementName: "text_main")]
    public string TextMain { get; set; } = string.Empty;
    
    [BsonElement(elementName: "inner_items_type")]
    public string? InnerItemsType { get; set; } = string.Empty;
    
    [BsonElement(elementName: "inner_items")]
    public IEnumerable<ContentItem> InnerItems { get; set; } = Array.Empty<ContentItem>();

    public IEnumerable<ContentItem> GetAllContentInArray()
    {
        var list = new List<ContentItem>();

        foreach (var contentItem in InnerItems)
        {
            list.AddRange(contentItem.GetAllContentInArray());
        }

        list.Add(this);

        return list;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Id.GetHashCode();
    }
}