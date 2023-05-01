using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.Search;

public interface ISearchHandler
{
    Task<IEnumerable<ProvisionHeader>> SearchProvisionsAsync(string keywords);
}