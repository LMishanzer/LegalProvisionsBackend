using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Search.Indexing.FulltextIndexing;
using LegalProvisionsLib.Search.Indexing.KeywordsIndexing;

namespace LegalProvisionsLib.Search.HeaderIndexing;

public class HeaderIndexManager : IHeaderIndexManager
{
    private readonly IKeywordsIndexer _keywordsIndexer;
    private readonly IFulltextIndexer _fulltextIndexer;

    public HeaderIndexManager(
        IKeywordsIndexer keywordsIndexer,
        IFulltextIndexer fulltextIndexer)
    {
        _keywordsIndexer = keywordsIndexer;
        _fulltextIndexer = fulltextIndexer;
    }
    
    public async Task IndexAsync(ProvisionHeaderFields headerFields, Guid provisionId)
    {
        await _keywordsIndexer.IndexRecordAsync(new KeywordsRecord
        {
            Text = headerFields.Title,
            ProvisionId = provisionId
        });
        
        foreach (var keyword in headerFields.Keywords)
        {
            await _keywordsIndexer.IndexRecordAsync(new KeywordsRecord
            {
                Text = keyword,
                ProvisionId = provisionId
            });
        }
    }

    public async Task RemoveKeywordsAsync(Guid headerId) => await _keywordsIndexer.DeleteByProvisionAsync(headerId);

    public async Task RemoveFullTextAsync(Guid headerId) => await _fulltextIndexer.DeleteByProvisionAsync(headerId);
}