using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionWarehouse.DataHandling.Header;
using LegalProvisionsLib.Search.HeaderIndexing;

namespace LegalProvisionsLib.ProvisionWarehouse.Header;

public class HeaderWarehouse : IHeaderWarehouse
{
    private readonly IHeaderHandler _headerHandler;
    private readonly IHeaderIndexManager _headerIndexManager;

    public HeaderWarehouse(
        IHeaderHandler headerHandler,
        IHeaderIndexManager headerIndexManager)
    {
        _headerHandler = headerHandler;
        _headerIndexManager = headerIndexManager;
    }
    
    public async Task<Guid> AddAsync(ProvisionHeaderFields headerFields)
    {
        var headerId = await _headerHandler.AddAsync(headerFields);
        await _headerIndexManager.IndexAsync(headerFields, headerId);

        return headerId;
    }

    public async Task<DataItem<ProvisionHeaderFields>> GetOneAsync(Guid headerId) => await _headerHandler.GetOneAsync(headerId);

    public async Task UpdateAsync(Guid headerId, ProvisionHeaderFields headerFields)
    {
        var updateTask = _headerHandler.UpdateAsync(headerId, headerFields);
        var refreshIndexTask = RefreshIndex(headerId, headerFields);

        await Task.WhenAll(updateTask, refreshIndexTask);
    }

    public async Task DeleteAsync(Guid headerId)
    {
        var deleteTask = _headerHandler.DeleteAsync(headerId);
        var removeDocsTask = RemoveIndexesDocuments(headerId);

        await Task.WhenAll(deleteTask, removeDocsTask);
    }

    public async Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync() => await _headerHandler.GetAllProvisionsAsync();

    public async Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request) => 
        await _headerHandler.GetProvisionHeadersAsync(request);

    private async Task RefreshIndex(Guid headerId, ProvisionHeaderFields headerFields)
    {
        await _headerIndexManager.RemoveKeywordsAsync(headerId);
        await _headerIndexManager.IndexAsync(headerFields, headerId);
    }
    
    private async Task RemoveIndexesDocuments(Guid headerId)
    {
        await _headerIndexManager.RemoveKeywordsAsync(headerId);
        await _headerIndexManager.RemoveFullTextAsync(headerId);
    }
}