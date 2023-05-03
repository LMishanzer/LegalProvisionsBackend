namespace LegalProvisionsLib.Search.Indexing.FulltextIndexing;

public record FulltextRecord : IRecord
{
    public required string Text { get; init; }
    public required Guid ProvisionId { get; init; }
    public required Guid VersionId { get; init; }
}