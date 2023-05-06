using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.DataHandling.Header;

public interface IHeaderHandler : IProvisionHandler<ProvisionHeaderFields>
{
    Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync();
    Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request);
}