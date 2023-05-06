using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionStorage.Header;

namespace LegalProvisionsLib.ProvisionStorage.DataHandling.Header;

public interface IHeaderHandler : IProvisionHandler<ProvisionHeaderFields>
{
    Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync();
    Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request);
}