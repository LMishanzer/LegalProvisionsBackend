using LegalProvisionsLib.DataHandling;
using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        var provisionVersion = await _provisionHandler.GetActualProvisionVersionAsync(id);

        return provisionVersion;
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
    public async Task<IActionResult> UpdateVersion(Guid versionId, 
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] ProvisionVersionFields? versionFields)
    {
        if (versionFields == null)
            return BadRequest("Version fields cannot be empty.");
            
        await _provisionHandler.UpdateVersionAsync(versionId, versionFields);

        return Ok();
    }
    
    [HttpPut("{headerId:guid}")]
    public async Task UpdateHeader(Guid headerId, [FromBody] ProvisionHeaderFields headerFields)
    {
        await _provisionHandler.UpdateHeaderAsync(headerId, headerFields);
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