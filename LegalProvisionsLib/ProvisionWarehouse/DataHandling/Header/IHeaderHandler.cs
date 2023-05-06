using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionWarehouse.Header;

namespace LegalProvisionsLib.ProvisionWarehouse.DataHandling.Header;

public interface IHeaderHandler : IProvisionHandler<ProvisionHeaderFields>
{
    Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync();
    Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request);
}