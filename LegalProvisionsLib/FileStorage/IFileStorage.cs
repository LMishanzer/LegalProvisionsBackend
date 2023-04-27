using LegalProvisionsLib.FileStorage.Models;

namespace LegalProvisionsLib.FileStorage;

public interface IFileStorage
{
    Task<FileToStoreInfo> AddFileAsync(FileToStore file);
    FileToStore GetFile(FileToStoreInfo fileInfo);
    void DeleteFile(FileToStoreInfo fileInfo);
}