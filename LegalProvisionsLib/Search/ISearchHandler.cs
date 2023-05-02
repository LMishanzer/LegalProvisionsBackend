namespace LegalProvisionsLib.Search;

public interface ISearchHandler
{
    IAsyncEnumerable<SearchResult> SearchProvisionsAsync(string keywords);
}