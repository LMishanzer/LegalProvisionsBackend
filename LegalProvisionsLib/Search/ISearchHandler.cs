using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Search.Indexing;

namespace LegalProvisionsLib.Search;

public interface ISearchHandler
{
    Task<IEnumerable<ProvisionHeader>> SearchProvisionsAsync(QueryModel request);
}