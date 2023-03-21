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
    
    [HttpGet("{id}")]
    public async Task<LegalProvision> GetOne(string id)
    {
        return await _dataPersistence.GetProvisionAsync(id);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LegalProvision provision)
    {
        var objectId = await _dataPersistence.AddProvisionAsync(provision);
        
        return Ok(objectId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] LegalProvisionUpdate provision)
    {
        await _dataPersistence.UpdateProvisionAsync(id, provision);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _dataPersistence.DeleteProvisionAsync(id);

        return Ok();
    }
}