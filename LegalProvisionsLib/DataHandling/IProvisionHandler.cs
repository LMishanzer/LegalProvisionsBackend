using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.DataHandling;

public interface IProvisionHandler<T> where T : class
{
    Task<Guid> AddAsync(T fields);
    Task<DataItem<T>> GetOneAsync(Guid id);
    Task UpdateAsync(Guid id, T fields);
    Task DeleteAsync(Guid id);
}