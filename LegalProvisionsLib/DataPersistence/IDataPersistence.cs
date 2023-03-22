using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.DataPersistence;

public interface IDataPersistence
{
    public Task<IEnumerable<LegalProvision>> GetAllProvisionsAsync();

    public Task<LegalProvision> GetProvisionAsync(Guid id);

    public Task<Guid> AddProvisionAsync(LegalProvisionFields provisionFields);

    public Task UpdateProvisionAsync(Guid id, LegalProvisionFields updatedProvisionFields);

    public Task DeleteProvisionAsync(Guid id);
}