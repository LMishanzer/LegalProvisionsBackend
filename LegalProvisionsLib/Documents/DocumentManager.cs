using LegalProvisionsLib.DataHandling;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Documents.Models;
using LegalProvisionsLib.Exceptions;
using LegalProvisionsLib.FileStorage;
using LegalProvisionsLib.FileStorage.Models;

namespace LegalProvisionsLib.Documents;

public class DocumentManager : IDocumentManager
{
    private readonly IProvisionHandler _provisionHandler;
    private readonly IFileStorage _fileStorage;

    public DocumentManager(IProvisionHandler provisionHandler, IFileStorage fileStorage)
    {
        _provisionHandler = provisionHandler;
        _fileStorage = fileStorage;
    }
    
    public async Task AddProvisionDocumentAsync(Document document)
    {
        var version = await _provisionHandler.GetProvisionVersionAsync(document.VersionId);
        
        if (version.Fields.FileMetadata is not null)
        {
            _fileStorage.DeleteFile(version.Fields.FileMetadata.NameInStorage);
        }
        
        var fileName = await _fileStorage.AddFileAsync(document.File.Content);
        
        version.Fields.FileMetadata = new FileMetadata(name: document.File.Name, nameInStorage: fileName) ;
        
        await _provisionHandler.UpdateVersionAsync(version.Id, version.Fields);
    }

    public async Task<FileToStore> GetProvisionDocumentAsync(Guid versionId)
    {
        var version = await _provisionHandler.GetProvisionVersionAsync(versionId);
        var fileMetadata = version.Fields.FileMetadata;

        if (fileMetadata is null)
            throw new InvalidFileNameException("No document is defined for this provision.");

        var fileData = _fileStorage.GetFile(fileMetadata.NameInStorage);

        return new FileToStore(fileMetadata.Name, fileData);
    }
}