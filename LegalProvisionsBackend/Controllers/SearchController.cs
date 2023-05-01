using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Search;
using LegalProvisionsLib.Search.Indexing;
using LegalProvisionsLib.Search.Indexing.KeywordsIndexing;
using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

[Route("/[controller]")]
public class SearchController : Controller
{
    private readonly ISearchHandler _searchHandler;

    public SearchController(ISearchHandler searchHandler)
    {
        _searchHandler = searchHandler;
    }
    
    [HttpPost]
    public async Task<ActionResult<IEnumerable<ProvisionHeader>>> SearchProvisions([FromBody] QueryModel? request)
    {
        if (string.IsNullOrWhiteSpace(request?.Keyword))
            return BadRequest();
        
        var result = await _searchHandler.SearchProvisionsAsync(request.Keyword);

        return Ok(result);
    }
}