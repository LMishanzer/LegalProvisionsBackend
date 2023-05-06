using LegalProvisionsLib.TextExtracting;
using MongoDB.Bson.Serialization.Attributes;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace LegalProvisionsLib.DataPersistence.Models;

public class ContentItem : ITextExtractable
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

    [BsonElement(elementName: "references")]
    public IEnumerable<Reference> References { get; set; } = Array.Empty<Reference>();

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

    IEnumerable<string> ITextExtractable.ExtractEntireText()
    {
        var textList = new List<string>{ TextMain };

        foreach (var innerItem in InnerItems)
        {
            var extractable = innerItem as ITextExtractable;
            textList.AddRange(extractable.ExtractEntireText());
        }

        return textList;
    }
}