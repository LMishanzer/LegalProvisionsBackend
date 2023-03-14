using System.Collections;
using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.DataPersistence;

public interface IDataPersistence
{
    public Task<IEnumerable<LegalProvision>> GetAllProvisionsAsync();

    public Task<LegalProvision> GetProvisionAsync(string id);

    public Task<string> AddProvisionAsync(LegalProvision provision);

    public Task UpdateProvisionAsync(LegalProvision provision);

    public Task DeleteProvisionAsync(string id);
}