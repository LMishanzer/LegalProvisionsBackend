using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.ProvisionWarehouse;

public interface IProvisionWarehouse<T> where T : class
{
    Task<Guid> AddAsync(T fields);
    Task<DataItem<T>> GetOneAsync(Guid headerId);
    Task UpdateAsync(Guid headerId, T headerFields);
    Task DeleteAsync(Guid versionId);
}