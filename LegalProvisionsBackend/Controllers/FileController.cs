using System.Net.Mime;
using LegalProvisionsLib.Documents;
using LegalProvisionsLib.Documents.Models;
using LegalProvisionsLib.FileStorage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

[Route("[controller]/{versionId:guid}")]
public class FileController : Controller
{
    private readonly IDocumentManager _documentManager;

    public FileController(IDocumentManager documentManager)
    {
        _documentManager = documentManager;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(Guid versionId, IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        await using var fileStream = file.OpenReadStream();
        await _documentManager.AddProvisionDocumentAsync(new Document
        (
            versionId: versionId,
            file: new FileToStore(file.FileName, fileStream)
        ));

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Download(Guid versionId)
    {
        FileToStore file;

        try
        {
            file = await _documentManager.GetProvisionDocumentAsync(versionId);
        }
        catch (FileNotFoundException e)
        {
            return BadRequest($"File {e.FileName} wasn't found.");
        }

        var contentDisposition = new ContentDisposition
        {
            FileName = file.Name,
            Size = file.Content.Length
        };
        Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

        return File(file.Content, "application/pdf");
    }
}