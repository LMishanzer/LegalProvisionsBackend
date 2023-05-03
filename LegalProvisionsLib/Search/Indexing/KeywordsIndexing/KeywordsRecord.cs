namespace LegalProvisionsLib.Search.Indexing.KeywordsIndexing;

public record KeywordsRecord : IRecord
{
    public required string Text { get; init; }
    public required Guid ProvisionId { get; init; }
}