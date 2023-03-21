using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.DataPersistence;

public interface IDataPersistence
{
    public Task<IEnumerable<LegalProvision>> GetAllProvisionsAsync();

    public Task<LegalProvision> GetProvisionAsync(Guid id);

    public Task<Guid> AddProvisionAsync(LegalProvision provision);

    public Task UpdateProvisionAsync(Guid id, LegalProvisionUpdate newProvision);

    public Task DeleteProvisionAsync(Guid id);
}