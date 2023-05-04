using LegalProvisionsLib.Logging;

namespace LegalProvisionsLib.FileStorage;

public class FilesystemStorage : IFileStorage
{
    private readonly ILogger _logger;
    private const string DirectoryPath = "AppFiles";
    private readonly DirectoryInfo _directory;

    public FilesystemStorage(ILogger logger)
    {
        _logger = logger;
        _directory = GetDirectory();
    }
    
    public async Task<string> AddFileAsync(Stream fileContent)
    {
        var newFileName = $"{Guid.NewGuid()}.pdf";
        var fullFileName = Path.Combine(_directory.FullName, newFileName);
        var fileInfo = new FileInfo(fullFileName);
        await using var fileStream = fileInfo.Create();
        await fileContent.CopyToAsync(fileStream);

        return newFileName;
    }
    
    public Stream GetFile(string fileName)
    {
        var fullFileName = Path.Combine(_directory.FullName, fileName);
        var fileInnerInfo = new FileInfo(fullFileName);

        if (!fileInnerInfo.Exists)
            throw new FileNotFoundException($"File {fileName} cannot be found.", fileName);

        var fileStream = fileInnerInfo.OpenRead();

        return fileStream;
    }

    public void DeleteFile(string fileName)
    {
        var fullFileName = Path.Combine(DirectoryPath, fileName);
        var fileInnerInfo = new FileInfo(fullFileName);

        try
        {
            fileInnerInfo.Delete();
        }
        catch (Exception e)
        {
            _logger.LogException(e);
        }
    }

    private static DirectoryInfo GetDirectory()
    {
        return !Directory.Exists(DirectoryPath) 
            ? Directory.CreateDirectory(DirectoryPath) 
            : new DirectoryInfo(DirectoryPath);
    }
}