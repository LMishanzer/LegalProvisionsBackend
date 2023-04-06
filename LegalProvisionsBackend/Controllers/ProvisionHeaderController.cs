using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.DataPersistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

[Route("[controller]/[action]")]
public class ProvisionHeaderController : Controller
{
    private readonly MongoPersistence _dataPersistence;

    public ProvisionHeaderController(MongoPersistence dataPersistence)
    {
        _dataPersistence = dataPersistence;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ProvisionHeader>> GetAll()
    {
        return await _dataPersistence.GetAllProvisionHeadersAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionHeader> GetOne(Guid id)
    {
        return await _dataPersistence.GetProvisionHeaderAsync(id);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProvisionHeaderFields provisionHeaderFields)
    {
        var objectId = await _dataPersistence.AddProvisionHeaderAsync(provisionHeaderFields);
        
        return Ok(objectId);
    }
}