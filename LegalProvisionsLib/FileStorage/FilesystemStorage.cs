using LegalProvisionsLib.FileStorage.Models;

namespace LegalProvisionsLib.FileStorage;

public class FilesystemStorage : IFileStorage
{
    private const string DirectoryPath = "AppFiles";
    private readonly DirectoryInfo _directory;

    public FilesystemStorage()
    {
        _directory = GetDirectory();
    }
    
    public async Task<FileToStoreInfo> AddFileAsync(FileToStore file)
    {
        var fullFileName = Path.Combine(_directory.FullName, file.Name);
        var fileInfo = new FileInfo(fullFileName);
        await using var fileStream = fileInfo.Create();
        await file.Content.CopyToAsync(fileStream);

        return new FileToStoreInfo(file.Name);
    }
    
    public FileToStore GetFile(FileToStoreInfo fileInfo)
    {
        var fullFileName = Path.Combine(_directory.FullName, fileInfo.Name);
        var fileInnerInfo = new FileInfo(fullFileName);

        if (!fileInnerInfo.Exists)
            throw new FileNotFoundException($"File {fileInfo.Name} cannot be found.", fileInfo.Name);

        var fileStream = fileInnerInfo.OpenRead();

        return new FileToStore(fileInfo.Name, fileStream);
    }

    public Task<FileToStore> GetFileAsync(FileToStoreInfo fileInfo)
    {
        return Task.FromResult(GetFile(fileInfo));
    }

    public void DeleteFile(FileToStoreInfo fileInfo)
    {
        var fullFileName = Path.Combine(DirectoryPath, fileInfo.Name);
        var fileInnerInfo = new FileInfo(fullFileName);
        
        if (!fileInnerInfo.Exists)
            throw new FileNotFoundException($"File {fileInfo.Name} cannot be found.", fileInfo.Name);

        fileInnerInfo.Delete();
    }

    private static DirectoryInfo GetDirectory()
    {
        return !Directory.Exists(DirectoryPath) 
            ? Directory.CreateDirectory(DirectoryPath) 
            : new DirectoryInfo(DirectoryPath);
    }
}