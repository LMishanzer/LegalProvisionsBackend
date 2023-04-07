using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences.Models;

namespace LegalProvisionsLib.DataHandling;

public interface IProvisionHandler
{
    Task<Guid> AddProvision(ProvisionHeaderFields headerFields);
    Task<Guid> AddProvisionVersion(ProvisionVersionFields versionFields);
    Task<IEnumerable<ProvisionHeader>> GetAllProvisions();
    Task<ProvisionHeader> GetProvisionHeader(Guid id);
    Task<ProvisionVersion> GetActualProvisionVersion(Guid id);
    Task<ProvisionVersion> GetProvisionVersion(Guid id, DateTime issueDate);
    Task<ProvisionDifference> GetVersionDifferences(Guid original, Guid changed);
}