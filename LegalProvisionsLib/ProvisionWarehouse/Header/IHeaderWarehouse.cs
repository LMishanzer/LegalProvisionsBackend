using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.ProvisionWarehouse.Header;

public interface IHeaderWarehouse : IProvisionWarehouse<ProvisionHeaderFields>
{
    Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync();
    Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request);
}