using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.ProvisionStorage.Version;

public interface IVersionStorage : IProvisionStorage<ProvisionVersionFields>
{
    Task<ProvisionVersion> GetActualProvisionVersionAsync(Guid headerId);
    Task<ProvisionVersion> GetProvisionVersionAsync(Guid headerId, DateTime issueDate);
}