namespace LegalProvisionsLib.FileStorage;

public interface IFileStorage
{
    Task<string> AddFileAsync(Stream file);
    Stream GetFile(string fileName);
    void DeleteFile(string fileName);
}