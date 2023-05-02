using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.Search;

public class SearchResult
{
    public required ProvisionHeader ProvisionHeader { get; init; }
    public Guid? VersionId { get; init; }
}