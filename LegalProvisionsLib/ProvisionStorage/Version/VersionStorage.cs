using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionStorage.DataHandling.Version;
using LegalProvisionsLib.Search.VersionIndexing;

namespace LegalProvisionsLib.ProvisionStorage.Version;

public class VersionStorage : IVersionStorage
{
    private readonly IVersionHandler _versionHandler;
    private readonly IVersionIndexManager _versionIndexManager;

    public VersionStorage(
        IVersionHandler versionHandler,
        IVersionIndexManager versionIndexManager)
    {
        _versionHandler = versionHandler;
        _versionIndexManager = versionIndexManager;
    }
    
    public async Task<Guid> AddAsync(ProvisionVersionFields fields)
    {
        var versionId = await _versionHandler.AddAsync(fields);
        var version = await _versionHandler.GetOneAsync(versionId);
        await _versionIndexManager.IndexAsync(provisionVersionFields: version.Fields, provisionId: version.Fields.ProvisionHeader, versionId: versionId);

        return versionId;
    }

    public async Task<DataItem<ProvisionVersionFields>> GetOneAsync(Guid versionId)
    {
        return await _versionHandler.GetOneAsync(versionId);
    }

    public async Task UpdateAsync(Guid versionId, ProvisionVersionFields versionFields)
    {
        var updateTask = _versionHandler.UpdateAsync(versionId, versionFields);
        var refreshIndexTask = RefreshIndex(versionId, versionFields);

        await Task.WhenAll(updateTask, refreshIndexTask);
    }
    
    public async Task DeleteAsync(Guid versionId)
    {
        var deleteTask = _versionHandler.DeleteAsync(versionId);
        var removeIndexTask = _versionIndexManager.RemoveFromIndexAsync(versionId);

        await Task.WhenAll(deleteTask, removeIndexTask);
    }

    public async Task<ProvisionVersion> GetActualProvisionVersionAsync(Guid headerId) => 
        await _versionHandler.GetActualProvisionVersionAsync(headerId);

    public async Task<ProvisionVersion> GetProvisionVersionAsync(Guid headerId, DateTime issueDate) => 
        await _versionHandler.GetProvisionVersionAsync(headerId, issueDate);

    private async Task RefreshIndex(Guid versionId, ProvisionVersionFields versionFields)
    {
        await _versionIndexManager.RemoveFromIndexAsync(versionId);
        await _versionIndexManager.IndexAsync(versionFields, versionFields.ProvisionHeader, versionId);
    }
}