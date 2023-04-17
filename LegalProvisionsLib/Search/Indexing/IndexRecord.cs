namespace LegalProvisionsLib.Search.Indexing;

public class IndexRecord
{
    public string Keyword { get; set; } = string.Empty;
    public Guid ProvisionId { get; set; }
}