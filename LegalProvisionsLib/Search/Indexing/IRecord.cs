namespace LegalProvisionsLib.Search.Indexing;

public interface IRecord
{
    public string Text { get; }
    public Guid ProvisionId { get; }
}