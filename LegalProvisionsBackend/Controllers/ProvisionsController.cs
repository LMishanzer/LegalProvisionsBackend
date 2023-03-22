using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.DataPersistence.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

[Route("[controller]/[action]")]
public class ProvisionsController : Controller
{
    private readonly IDataPersistence _dataPersistence;

    public ProvisionsController(IDataPersistence dataPersistence)
    {
        _dataPersistence = dataPersistence;
    }

    [HttpGet]
    public async Task<IEnumerable<LegalProvision>> GetAll()
    {
        return await _dataPersistence.GetAllProvisionsAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<LegalProvision> GetOne(Guid id)
    {
        return await _dataPersistence.GetProvisionAsync(id);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LegalProvisionFields provisionFields)
    {
        var objectId = await _dataPersistence.AddProvisionAsync(provisionFields);
        
        return Ok(objectId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] LegalProvisionFields provisionFields)
    {
        await _dataPersistence.UpdateProvisionAsync(id, provisionFields);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _dataPersistence.DeleteProvisionAsync(id);

        return Ok();
    }
}