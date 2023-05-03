using LegalProvisionsLib.DataHandling;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Search.Indexing;
using LegalProvisionsLib.Search.Indexing.FulltextIndexing;
using LegalProvisionsLib.Search.Indexing.KeywordsIndexing;

namespace LegalProvisionsLib.Search;

public class SearchHandler : ISearchHandler
{
    private readonly IKeywordsIndexer _keywordsIndexer;
    private readonly IFulltextIndexer _fulltextIndexer;
    private readonly IProvisionHandler _provisionHandler;

    public SearchHandler(
        IKeywordsIndexer keywordsIndexer,
        IFulltextIndexer fulltextIndexer,
        IProvisionHandler provisionHandler)
    {
        _keywordsIndexer = keywordsIndexer;
        _fulltextIndexer = fulltextIndexer;
        _provisionHandler = provisionHandler;
    }
    
    public async IAsyncEnumerable<SearchResult> SearchProvisionsAsync(string keywords)
    {
        IEnumerable<IRecord> byKeywords = await _keywordsIndexer.GetByKeywordsAsync(keywords);
        IEnumerable<IRecord> byFulltext = await _fulltextIndexer.GetByKeywordsAsync(keywords);

        var allResults = byKeywords.Union(byFulltext).DistinctBy(p => p.ProvisionId);

        foreach (var indexRecord in allResults)
        {
            var header = await GetHeaderSafeAsync(indexRecord.ProvisionId);
            
            if (header == null)
                continue;

            yield return new SearchResult
            {
                ProvisionHeader = header,
                VersionId = indexRecord switch
                {
                    FulltextRecord fulltextRecord => fulltextRecord.VersionId,
                    _ => null
                }
            };
        }
    }

    private async Task<ProvisionHeader?> GetHeaderSafeAsync(Guid provisionId)
    {
        try
        {
            return await _provisionHandler.GetProvisionHeaderAsync(provisionId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}