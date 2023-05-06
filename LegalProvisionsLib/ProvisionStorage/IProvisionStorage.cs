using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.ProvisionStorage;

public interface IProvisionStorage<T> where T : class
{
    Task<Guid> AddAsync(T fields);
    Task<DataItem<T>> GetOneAsync(Guid headerId);
    Task UpdateAsync(Guid headerId, T headerFields);
    Task DeleteAsync(Guid versionId);
}