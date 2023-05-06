using LegalProvisionsLib.TextExtracting;

namespace LegalProvisionsLib.Search.VersionIndexing;

public interface IVersionIndexManager
{
    Task IndexAsync(ITextExtractable provisionVersionFields, Guid provisionId, Guid versionId);

    Task RemoveFromIndexAsync(Guid versionId);
}