using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.Search.HeaderIndexing;

public interface IHeaderIndexManager
{
    Task IndexAsync(ProvisionHeaderFields headerFields, Guid provisionId);
    Task RemoveKeywordsAsync(Guid headerId);
    Task RemoveFullTextAsync(Guid headerId);
}