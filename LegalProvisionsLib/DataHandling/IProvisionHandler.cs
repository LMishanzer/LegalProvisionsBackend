using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences.Models;

namespace LegalProvisionsLib.DataHandling;

public interface IProvisionHandler
{
    Task<Guid> AddProvisionAsync(ProvisionHeaderFields headerFields);
    Task<Guid> AddProvisionVersionAsync(ProvisionVersionFields versionFields);
    Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync();
    Task<ProvisionHeader> GetProvisionHeaderAsync(Guid id);
    Task<ProvisionVersion> GetActualProvisionVersionAsync(Guid id);
    Task<ProvisionVersion> GetProvisionVersionAsync(Guid id, DateTime issueDate);
    Task<ProvisionVersion> GetProvisionVersionAsync(Guid versionId);
    Task<ProvisionDifference> GetVersionDifferencesAsync(Guid original, Guid changed);
    Task<ProvisionDifference> GetVersionDifferencesAsync(DifferenceRequest differenceRequest);
    Task UpdateVersionAsync(Guid versionId, ProvisionVersionFields versionFields);
    Task DeleteProvisionAsync(Guid headerId);
    Task DeleteProvisionVersionAsync(Guid versionId);
}