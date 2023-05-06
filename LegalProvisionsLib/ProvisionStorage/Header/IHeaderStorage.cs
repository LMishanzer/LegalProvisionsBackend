using LegalProvisionsLib.DataHandling.Header;
using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.ProvisionStorage.Header;

public interface IHeaderStorage : IProvisionStorage<ProvisionHeaderFields>
{
    Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync();
    Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request);
}