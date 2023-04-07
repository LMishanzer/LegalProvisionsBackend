using LegalProvisionsLib.DataHandling;
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
        return await _provisionHandler.GetAllProvisions();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionVersion> GetActualVersion(Guid id)
    {
        return await _provisionHandler.GetActualProvisionVersion(id);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionHeader> GetProvisionHeader(Guid id)
    {
        return await _provisionHandler.GetProvisionHeader(id);
    }
    
    [HttpGet("{id:guid}/{issueDate:datetime}")]
    public async Task<ProvisionVersion> GetProvisionVersion(Guid id, DateTime issueDate)
    {
        return await _provisionHandler.GetProvisionVersion(id, issueDate);
    }

    [HttpPost]
    public async Task<Guid> AddProvision([FromBody] ProvisionHeaderFields headerFields)
    {
        return await _provisionHandler.AddProvision(headerFields);
    }
    
    [HttpPost]
    public async Task<Guid> AddProvisionVersion([FromBody] ProvisionVersionFields headerFields)
    {
        return await _provisionHandler.AddProvisionVersion(headerFields);
    }

    [HttpGet("{originalId:guid}/{changedId:guid}")]
    public async Task<ProvisionDifference> GetDifferences(Guid originalId, Guid changedId)
    {
        return await _provisionHandler.GetVersionDifferences(originalId, changedId);
    }
}