using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.Differences;
using LegalProvisionsLib.Differences.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

[Route("/provision/[action]")]
public class DifferenceController : Controller
{
    private readonly IDifferenceManager _differenceManager;

    public DifferenceController(IDifferenceManager differenceManager)
    {
        _differenceManager = differenceManager;
    }
    
    [HttpGet("{originalId:guid}/{changedId:guid}")]
    public async Task<ProvisionDifference> GetDifferences(Guid originalId, Guid changedId)
    {
        return await _differenceManager.GetVersionDifferencesAsync(originalId, changedId);
    }

    [HttpPost]
    public async Task<ProvisionDifference> GetDifferences([FromBody] DifferenceRequest differenceRequest)
    {
        return await _differenceManager.GetVersionDifferencesAsync(differenceRequest);
    }
}