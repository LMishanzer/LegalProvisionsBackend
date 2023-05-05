using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.DataHandling;

public interface IProvisionHandler
{
    Task<Guid> AddProvisionAsync(ProvisionHeaderFields headerFields);
    Task<Guid> AddProvisionVersionAsync(ProvisionVersionFields versionFields);
    Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync();
    Task<ProvisionHeader> GetProvisionHeaderAsync(Guid id);
    Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request);
    Task<ProvisionVersion> GetActualProvisionVersionAsync(Guid id);
    Task<ProvisionVersion> GetProvisionVersionAsync(Guid id, DateTime issueDate);
    Task<ProvisionVersion> GetProvisionVersionAsync(Guid versionId);
    Task UpdateVersionAsync(Guid versionId, ProvisionVersionFields versionFields);
    Task UpdateHeaderAsync(Guid headerId, ProvisionHeaderFields headerFields);
    Task DeleteProvisionAsync(Guid headerId);
    Task DeleteProvisionVersionAsync(Guid versionId);
}