using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Search;
using LegalProvisionsLib.Search.Indexing;
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
    public async Task<IEnumerable<ProvisionHeader>> SearchProvisions([FromBody] QueryModel request)
    {
        var result = await _searchHandler.SearchProvisionsAsync(request);

        return result;
    }
}