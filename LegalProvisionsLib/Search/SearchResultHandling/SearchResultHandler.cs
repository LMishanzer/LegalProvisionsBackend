using LegalProvisionsLib.Search.Indexing;
using LegalProvisionsLib.Search.Indexing.Fulltext;
using LegalProvisionsLib.Search.Indexing.Keywords;

namespace LegalProvisionsLib.Search.SearchResultHandling;

public class SearchResultHandler : ISearchResultHandler
{
    private readonly IKeywordsIndexer _keywordsIndexer;
    private readonly IFulltextIndexer _fulltextIndexer;

    public SearchResultHandler(
        IKeywordsIndexer keywordsIndexer,
        IFulltextIndexer fulltextIndexer)
    {
        _keywordsIndexer = keywordsIndexer;
        _fulltextIndexer = fulltextIndexer;
    }
    
    public async Task<IEnumerable<IRecord>> GetSearchResultsAsync(string keywords)
    {
        var keywordsResultTask = _keywordsIndexer.GetByKeywordsAsync(keywords);
        var fulltextResultTask = _fulltextIndexer.GetByKeywordsAsync(keywords);
        await Task.WhenAll(keywordsResultTask, fulltextResultTask);

        var keywordsResult = (IEnumerable<IRecord>)keywordsResultTask.Result;
        var fulltextResult = (IEnumerable<IRecord>)fulltextResultTask.Result;

        // concat the lists without duplicities
        var allResults = keywordsResult.UnionBy(fulltextResult, p => p.ProvisionId);

        return allResults;
    }
}