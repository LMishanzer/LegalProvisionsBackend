namespace LegalProvisionsLib.Search.Indexing.FulltextIndexing;

public class FulltextRecord : IRecord
{
    public required string FullText { get; init; }
    public required Guid ProvisionId { get; init; }

    string IRecord.Text => FullText;
}