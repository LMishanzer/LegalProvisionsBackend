using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionStorage.Header;
using LegalProvisionsLib.Search.Indexing.FulltextIndexing;
using LegalProvisionsLib.Search.SearchResultHandling;

namespace LegalProvisionsLib.Search;

public class SearchHandler : ISearchHandler
{
    private readonly ISearchResultHandler _searchResultHandler;
    private readonly IHeaderStorage _provisionStorage;

    public SearchHandler(
        ISearchResultHandler searchResultHandler,
        IHeaderStorage provisionStorage)
    {
        _searchResultHandler = searchResultHandler;
        _provisionStorage = provisionStorage;
    }
    
    public async IAsyncEnumerable<SearchResult> SearchProvisionsAsync(string keywords)
    {
        var searchResults = await _searchResultHandler.GetSearchResultsAsync(keywords);

        foreach (var indexRecord in searchResults)
        {
            var header = await GetHeaderSafeAsync(indexRecord.ProvisionId);
            
            if (header is null)
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
            return await _provisionStorage.GetOneAsync(provisionId) as ProvisionHeader;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}