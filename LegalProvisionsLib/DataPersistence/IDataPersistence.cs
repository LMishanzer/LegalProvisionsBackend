using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.DataPersistence;

public interface IDataPersistence
{
    public Task<IEnumerable<ProvisionVersion>> GetAllProvisionsAsync();

    public Task<ProvisionVersion> GetProvisionAsync(Guid id);

    public Task<Guid> AddProvisionAsync(ProvisionVersionFields provisionVersionFields);

    public Task UpdateProvisionAsync(Guid id, ProvisionVersionFields updatedProvisionVersionFields);

    public Task DeleteProvisionAsync(Guid id);

    public Task DeleteAllProvisionsAsync();
}