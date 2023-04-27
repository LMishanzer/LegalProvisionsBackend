using System.Net.Mime;
using LegalProvisionsLib.FileStorage;
using LegalProvisionsLib.FileStorage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegalProvisionsBackend.Controllers;

[Route("[controller]")]
public class FileController : Controller
{
    private readonly IFileStorage _fileStorage;
    
    public FileController(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest();

        await using var fileStream = file.OpenReadStream();
        await _fileStorage.AddFileAsync(new FileToStore(file.FileName, fileStream));

        return Ok();
    }

    [HttpGet("{fileName}")]
    public IActionResult Download(string fileName)
    {
        FileToStore file;

        try
        {
            file = _fileStorage.GetFile(new FileToStoreInfo(fileName));
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

    [HttpDelete("{fileName}")]
    public IActionResult Delete(string fileName)
    {
        try
        {
            _fileStorage.DeleteFile(new FileToStoreInfo(fileName));
        }
        catch (FileNotFoundException e)
        {
            return BadRequest($"File {e.FileName} wasn't found.");
        }

        return Ok();
    }
}