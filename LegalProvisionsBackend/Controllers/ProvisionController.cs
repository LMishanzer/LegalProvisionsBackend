using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionWarehouse.Header;
using LegalProvisionsLib.ProvisionWarehouse.Version;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LegalProvisionsBackend.Controllers;

[Route("[controller]/[action]")]
public class ProvisionController : Controller
{
    private readonly IVersionWarehouse _versionWarehouse;
    private readonly IHeaderWarehouse _headerWarehouse;

    public ProvisionController(
        IVersionWarehouse versionWarehouse,
        IHeaderWarehouse headerWarehouse)
    {
        _versionWarehouse = versionWarehouse;
        _headerWarehouse = headerWarehouse;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ProvisionHeader>> GetAll()
    {
        return await _headerWarehouse.GetAllProvisionsAsync();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionVersion> GetActualVersion(Guid id)
    {
        var provisionVersion = await _versionWarehouse.GetActualProvisionVersionAsync(id);

        return provisionVersion;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ProvisionHeader?> GetProvisionHeader(Guid id)
    {
        return await _headerWarehouse.GetOneAsync(id) as ProvisionHeader;
    }
    
    [HttpPost]
    public async Task<IEnumerable<ProvisionHeader>> GetProvisionHeaders([FromBody] ProvisionHeadersRequest request)
    {
        return await _headerWarehouse.GetProvisionHeadersAsync(request);
    }
    
    [HttpGet("{id:guid}/{issueDate:datetime}")]
    public async Task<ProvisionVersion> GetProvisionVersion(Guid id, DateTime issueDate)
    {
        return await _versionWarehouse.GetProvisionVersionAsync(id, issueDate);
    }
    
    [HttpGet("{versionId:guid}")]
    public async Task<ProvisionVersion?> GetProvisionVersion(Guid versionId)
    {
        return await _versionWarehouse.GetOneAsync(versionId) as ProvisionVersion;
    }

    [HttpPost]
    public async Task<Guid> AddProvision([FromBody] ProvisionHeaderFields headerFields)
    {
        return await _headerWarehouse.AddAsync(headerFields);
    }
    
    [HttpPost]
    [Consumes("application/json")]
    public async Task<Guid> AddProvisionVersion([FromBody] ProvisionVersionFields versionFields)
    {
        return await _versionWarehouse.AddAsync(versionFields);
    }

    [HttpPut("{versionId:guid}")]
    public async Task<IActionResult> UpdateVersion(Guid versionId, 
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] ProvisionVersionFields? versionFields)
    {
        if (versionFields == null)
            return BadRequest("Version fields cannot be empty.");
            
        await _versionWarehouse.UpdateAsync(versionId, versionFields);

        return Ok();
    }
    
    [HttpPut("{headerId:guid}")]
    public async Task UpdateHeader(Guid headerId, [FromBody] ProvisionHeaderFields headerFields)
    {
        await _headerWarehouse.UpdateAsync(headerId, headerFields);
    }

    [HttpDelete("{headerId:guid}")]
    public async Task DeleteProvision(Guid headerId)
    {
        await _headerWarehouse.DeleteAsync(headerId);
    }

    [HttpDelete("{versionId:guid}")]
    public async Task DeleteProvisionVersion(Guid versionId)
    {
        await _versionWarehouse.DeleteAsync(versionId);
    }
}