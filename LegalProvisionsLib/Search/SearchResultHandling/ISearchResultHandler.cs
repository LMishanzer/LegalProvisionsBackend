using LegalProvisionsLib.Search.Indexing;

namespace LegalProvisionsLib.Search.SearchResultHandling;

public interface ISearchResultHandler
{
    Task<IEnumerable<IRecord>> GetSearchResultsAsync(string keywords);
}