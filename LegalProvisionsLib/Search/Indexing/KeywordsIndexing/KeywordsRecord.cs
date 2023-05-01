namespace LegalProvisionsLib.Search.Indexing.KeywordsIndexing;

public class KeywordsRecord : IRecord
{
    public string Keywords { get; set; } = string.Empty;
    public Guid ProvisionId { get; set; }
    
    string IRecord.Text => Keywords;
}