using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.ProvisionStorage.DataHandling.Version;

public interface IVersionHandler : IProvisionHandler<ProvisionVersionFields>
{
    Task<ProvisionVersion> GetActualProvisionVersionAsync(Guid versionId);
    Task<ProvisionVersion> GetProvisionVersionAsync(Guid headerId, DateTime issueDate);
}