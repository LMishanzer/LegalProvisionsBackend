namespace LegalProvisionsLib.Search.Indexing.Keywords;

public record KeywordsRecord : IRecord
{
    public required string Text { get; init; }
    public required Guid ProvisionId { get; init; }
}