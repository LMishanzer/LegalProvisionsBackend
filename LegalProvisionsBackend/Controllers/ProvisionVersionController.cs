using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.DataPersistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

[Route("[controller]/[action]")]
public class ProvisionVersionController : Controller
{
    private readonly MongoPersistence _dataPersistence;

    public ProvisionVersionController(MongoPersistence dataPersistence)
    {
        _dataPersistence = dataPersistence;
    }

    [HttpGet]
    public async Task<IEnumerable<ProvisionVersion>> GetAll()
    {
        return await _dataPersistence.GetAllProvisionsAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionVersion> GetOne(Guid id)
    {
        return await _dataPersistence.GetProvisionAsync(id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IEnumerable<ProvisionVersion>> GetByHeaderId(Guid id)
    {
        return await _dataPersistence.GetVersionsByHeaderIdAsync(id);
    }

    [HttpGet("{headerId:guid}")]
    public async Task<ProvisionVersion> GetActualProvisionVersion(Guid headerId)
    {
        return await _dataPersistence.GetActualVersionAsync(headerId);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProvisionVersionFields provisionVersionFields)
    {
        var objectId = await _dataPersistence.AddProvisionAsync(provisionVersionFields);
        
        return Ok(objectId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProvisionVersionFields provisionVersionFields)
    {
        await _dataPersistence.UpdateProvisionAsync(id, provisionVersionFields);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _dataPersistence.DeleteProvisionAsync(id);

        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        await _dataPersistence.DeleteAllProvisionsAsync();

        return Ok();
    }
}