using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionWarehouse.Header;
using LegalProvisionsLib.Search.Indexing.Fulltext;
using LegalProvisionsLib.Search.SearchResultHandling;

namespace LegalProvisionsLib.Search;

public class SearchHandler : ISearchHandler
{
    private readonly ISearchResultHandler _searchResultHandler;
    private readonly IHeaderWarehouse _provisionWarehouse;

    public SearchHandler(
        ISearchResultHandler searchResultHandler,
        IHeaderWarehouse provisionWarehouse)
    {
        _searchResultHandler = searchResultHandler;
        _provisionWarehouse = provisionWarehouse;
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
            return await _provisionWarehouse.GetOneAsync(provisionId) as ProvisionHeader;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}