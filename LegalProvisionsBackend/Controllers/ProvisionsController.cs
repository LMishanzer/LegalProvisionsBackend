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
    public async Task<IEnumerable<LegalProvision>> GetAllProvisions()
    {
        return await _dataPersistence.GetAllProvisionsAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<LegalProvision> GetProvision(string id)
    {
        return await _dataPersistence.GetProvisionAsync(id);
    }
    
    [HttpPost]
    public async Task CreateProvision([FromBody] LegalProvision provision)
    {
        await _dataPersistence.AddProvisionAsync(provision);
    }
}