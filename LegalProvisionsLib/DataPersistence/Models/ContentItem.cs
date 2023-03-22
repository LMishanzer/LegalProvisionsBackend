using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class ContentItem
{
    [BsonElement(elementName: "name")]
    public string Name { get; set; }
    
    [BsonElement(elementName: "title")]
    public string? Title { get; set; }

    [BsonElement(elementName: "text_main")]
    public string? TextMain { get; set; }
    
    [BsonElement(elementName: "inner_items")]
    public IEnumerable<ContentItem>? InnerItems { get; set; }

    public void ValidateStructure()
    {
        if (InnerItems == null || !InnerItems.Any())
            return;

        var elNames = InnerItems.First().Name;
        
        foreach (var innerItem in InnerItems)
        {
            innerItem.ValidateStructure();

            if (!elNames.Equals(innerItem.Name))
            {
                throw new NotImplementedException();
            }
        }
    }
}