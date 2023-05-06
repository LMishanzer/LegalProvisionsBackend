using LegalProvisionsLib.Search.Indexing;

namespace LegalProvisionsLib.Search.SearchResultsHandling;

public interface ISearchResultHandler
{
    Task<IEnumerable<IRecord>> GetSearchResultsAsync(string keywords);
}