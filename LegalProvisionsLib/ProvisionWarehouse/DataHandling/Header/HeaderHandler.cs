using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.FileStorage;
using LegalProvisionsLib.ProvisionWarehouse.Header;

namespace LegalProvisionsLib.ProvisionWarehouse.DataHandling.Header;

public class HeaderHandler : IHeaderHandler
{
    private readonly ProvisionVersionPersistence _provisionVersionPersistence;
    private readonly ProvisionHeaderPersistence _provisionHeaderPersistence;
    private readonly IFileStorage _fileStorage;

    public HeaderHandler(
        ProvisionVersionPersistence provisionVersionPersistence,
        ProvisionHeaderPersistence provisionHeaderPersistence,
        IFileStorage fileStorage)
    {
        _provisionVersionPersistence = provisionVersionPersistence;
        _provisionHeaderPersistence = provisionHeaderPersistence;
        _fileStorage = fileStorage;
    }
    
    public async Task<Guid> AddAsync(ProvisionHeaderFields headerFields) => 
        await _provisionHeaderPersistence.AddProvisionHeaderAsync(headerFields);

    public async Task<DataItem<ProvisionHeaderFields>> GetOneAsync(Guid headerId) => 
        await _provisionHeaderPersistence.GetProvisionHeaderAsync(headerId);

    public async Task UpdateAsync(Guid headerId, ProvisionHeaderFields headerFields) => 
        await _provisionHeaderPersistence.UpdateProvisionHeaderAsync(headerId, headerFields);

    public async Task DeleteAsync(Guid headerId)
    {
        var versions = await _provisionVersionPersistence.GetVersionsByHeaderIdAsync(headerId);

        var documents = versions
            .Select(v => v.Fields.FileMetadata)
            .Where(m => m is not null);

        foreach (var document in documents)
        {
            _fileStorage.DeleteFile(document!.NameInStorage);
        }
        
        await _provisionVersionPersistence.DeleteVersionsByHeaderAsync(headerId);
        await _provisionHeaderPersistence.DeleteProvisionHeaderAsync(headerId);
    }

    public async Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync()
    {
        return await _provisionHeaderPersistence.GetAllProvisionHeadersAsync();
    }

    public async Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request)
    {
        return await _provisionHeaderPersistence.GetProvisionHeadersAsync(request.ProvisionIds);
    }
}