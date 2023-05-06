using LegalProvisionsLib.Search.Indexing.Fulltext;
using LegalProvisionsLib.TextExtracting;

namespace LegalProvisionsLib.Search.VersionIndexing;

public class VersionIndexManager : IVersionIndexManager
{
    private readonly IFulltextIndexer _fulltextIndexer;

    public VersionIndexManager(IFulltextIndexer fulltextIndexer)
    {
        _fulltextIndexer = fulltextIndexer;
    }
    
    public async Task IndexAsync(ITextExtractable provisionVersionFields, Guid provisionId, Guid versionId)
    {
        var entireText = string.Join(" ", provisionVersionFields.ExtractEntireText());
        
        await _fulltextIndexer.IndexRecordAsync(new FulltextRecord
        {
            Text = entireText,
            ProvisionId = provisionId,
            VersionId = versionId
        });
    }

    public async Task RemoveFromIndexAsync(Guid versionId) => await _fulltextIndexer.DeleteByVersion(versionId);
}