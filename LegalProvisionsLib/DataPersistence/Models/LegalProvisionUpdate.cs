using MongoDB.Bson.Serialization.Attributes;

namespace LegalProvisionsLib.DataPersistence.Models;

public class LegalProvisionUpdate
{
    public string Title { get; set; }

    public IEnumerable<Article> Articles { get; set; }
}