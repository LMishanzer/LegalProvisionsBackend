using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

public class PingController : Controller
{
    [HttpGet("/ping")]
    public IActionResult Get()
    {
        return Ok("Server works!");
    }
}