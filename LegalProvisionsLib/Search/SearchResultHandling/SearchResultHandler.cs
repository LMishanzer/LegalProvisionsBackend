using LegalProvisionsLib.Search.Indexing;
using LegalProvisionsLib.Search.Indexing.FulltextIndexing;
using LegalProvisionsLib.Search.Indexing.KeywordsIndexing;

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

        // concat the lists, Concat will preserve the order of the elements
        var allResultsUnion = keywordsResult.Concat(fulltextResult);
        var withoutDuplicities = allResultsUnion.DistinctBy(p => p.ProvisionId);

        return withoutDuplicities;
    }
}