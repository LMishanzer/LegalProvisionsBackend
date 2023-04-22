using LegalProvisionsLib.DataHandling;
using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

[Route("[controller]/[action]")]
public class ProvisionController : Controller
{
    private readonly IProvisionHandler _provisionHandler;

    public ProvisionController(IProvisionHandler provisionHandler)
    {
        _provisionHandler = provisionHandler;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ProvisionHeader>> GetAll()
    {
        return await _provisionHandler.GetAllProvisionsAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionVersion> GetActualVersion(Guid id)
    {
        return await _provisionHandler.GetActualProvisionVersionAsync(id);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionHeader> GetProvisionHeader(Guid id)
    {
        return await _provisionHandler.GetProvisionHeaderAsync(id);
    }
    
    [HttpPost]
    public async Task<IEnumerable<ProvisionHeader>> GetProvisionHeaders([FromBody] ProvisionHeadersRequest request)
    {
        return await _provisionHandler.GetProvisionHeadersAsync(request);
    }
    
    [HttpGet("{id:guid}/{issueDate:datetime}")]
    public async Task<ProvisionVersion> GetProvisionVersion(Guid id, DateTime issueDate)
    {
        return await _provisionHandler.GetProvisionVersionAsync(id, issueDate);
    }
    
    [HttpGet("{versionId:guid}")]
    public async Task<ProvisionVersion> GetProvisionVersion(Guid versionId)
    {
        return await _provisionHandler.GetProvisionVersionAsync(versionId);
    }

    [HttpPost]
    public async Task<Guid> AddProvision([FromBody] ProvisionHeaderFields headerFields)
    {
        return await _provisionHandler.AddProvisionAsync(headerFields);
    }
    
    [HttpPost]
    [Consumes("application/json")]
    public async Task<Guid> AddProvisionVersion([FromBody] ProvisionVersionFields versionFields)
    {
        return await _provisionHandler.AddProvisionVersionAsync(versionFields);
    }

    [HttpGet("{originalId:guid}/{changedId:guid}")]
    public async Task<ProvisionDifference> GetDifferences(Guid originalId, Guid changedId)
    {
        return await _provisionHandler.GetVersionDifferencesAsync(originalId, changedId);
    }

    [HttpPost]
    public async Task<ProvisionDifference> GetDifferences([FromBody] DifferenceRequest differenceRequest)
    {
        return await _provisionHandler.GetVersionDifferencesAsync(differenceRequest);
    }

    [HttpPut("{versionId:guid}")]
    public async Task UpdateVersion(Guid versionId, [FromBody] ProvisionVersionFields versionFields)
    {
        await _provisionHandler.UpdateVersionAsync(versionId, versionFields);
    }

    [HttpDelete("{headerId:guid}")]
    public async Task DeleteProvision(Guid headerId)
    {
        await _provisionHandler.DeleteProvisionAsync(headerId);
    }

    [HttpDelete("{versionId:guid}")]
    public async Task DeleteProvisionVersion(Guid versionId)
    {
        await _provisionHandler.DeleteProvisionVersionAsync(versionId);
    }
}