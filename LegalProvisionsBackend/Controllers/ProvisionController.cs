using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionStorage.Header;
using LegalProvisionsLib.ProvisionStorage.Version;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LegalProvisionsBackend.Controllers;

[Route("[controller]/[action]")]
public class ProvisionController : Controller
{
    private readonly IVersionStorage _versionStorage;
    private readonly IHeaderStorage _headerStorage;

    public ProvisionController(
        IVersionStorage versionStorage,
        IHeaderStorage headerStorage)
    {
        _versionStorage = versionStorage;
        _headerStorage = headerStorage;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ProvisionHeader>> GetAll()
    {
        return await _headerStorage.GetAllProvisionsAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionVersion> GetActualVersion(Guid id)
    {
        var provisionVersion = await _versionStorage.GetActualProvisionVersionAsync(id);

        return provisionVersion;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionHeader?> GetProvisionHeader(Guid id)
    {
        return await _headerStorage.GetOneAsync(id) as ProvisionHeader;
    }
    
    [HttpPost]
    public async Task<IEnumerable<ProvisionHeader>> GetProvisionHeaders([FromBody] ProvisionHeadersRequest request)
    {
        return await _headerStorage.GetProvisionHeadersAsync(request);
    }
    
    [HttpGet("{id:guid}/{issueDate:datetime}")]
    public async Task<ProvisionVersion> GetProvisionVersion(Guid id, DateTime issueDate)
    {
        return await _versionStorage.GetProvisionVersionAsync(id, issueDate);
    }
    
    [HttpGet("{versionId:guid}")]
    public async Task<ProvisionVersion?> GetProvisionVersion(Guid versionId)
    {
        return await _versionStorage.GetOneAsync(versionId) as ProvisionVersion;
    }

    [HttpPost]
    public async Task<Guid> AddProvision([FromBody] ProvisionHeaderFields headerFields)
    {
        return await _headerStorage.AddAsync(headerFields);
    }
    
    [HttpPost]
    [Consumes("application/json")]
    public async Task<Guid> AddProvisionVersion([FromBody] ProvisionVersionFields versionFields)
    {
        return await _versionStorage.AddAsync(versionFields);
    }

    [HttpPut("{versionId:guid}")]
    public async Task<IActionResult> UpdateVersion(Guid versionId, 
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] ProvisionVersionFields? versionFields)
    {
        if (versionFields == null)
            return BadRequest("Version fields cannot be empty.");
            
        await _versionStorage.UpdateAsync(versionId, versionFields);

        return Ok();
    }
    
    [HttpPut("{headerId:guid}")]
    public async Task UpdateHeader(Guid headerId, [FromBody] ProvisionHeaderFields headerFields)
    {
        await _headerStorage.UpdateAsync(headerId, headerFields);
    }

    [HttpDelete("{headerId:guid}")]
    public async Task DeleteProvision(Guid headerId)
    {
        await _headerStorage.DeleteAsync(headerId);
    }

    [HttpDelete("{versionId:guid}")]
    public async Task DeleteProvisionVersion(Guid versionId)
    {
        await _versionStorage.DeleteAsync(versionId);
    }
}