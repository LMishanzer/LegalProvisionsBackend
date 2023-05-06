using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.ProvisionWarehouse.Version;

public interface IVersionWarehouse : IProvisionWarehouse<ProvisionVersionFields>
{
    Task<ProvisionVersion> GetActualProvisionVersionAsync(Guid headerId);
    Task<ProvisionVersion> GetProvisionVersionAsync(Guid headerId, DateTime issueDate);
}