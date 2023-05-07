using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Documents.Models;
using LegalProvisionsLib.Exceptions;
using LegalProvisionsLib.FileStorage;
using LegalProvisionsLib.ProvisionWarehouse.Version;

namespace LegalProvisionsLib.Documents;

public class DocumentManager : IDocumentManager
{
    private readonly IVersionWarehouse _versionWarehouse;
    private readonly IFileStorage _fileStorage;

    public DocumentManager(IVersionWarehouse versionWarehouse, IFileStorage fileStorage)
    {
        _versionWarehouse = versionWarehouse;
        _fileStorage = fileStorage;
    }
    
    public async Task AddProvisionDocumentAsync(Document document)
    {
        var version = await _versionWarehouse.GetOneAsync(document.VersionId);
        
        if (version.Fields.FileMetadata is not null)
        {
            _fileStorage.DeleteFile(version.Fields.FileMetadata.NameInStorage);
        }
        
        var fileName = await _fileStorage.AddFileAsync(document.File.Content);
        
        version.Fields.FileMetadata = new FileMetadata(name: document.File.Name, nameInStorage: fileName) ;
        
        await _versionWarehouse.UpdateAsync(version.Id, version.Fields);
    }

    public async Task<FileToStore> GetProvisionDocumentAsync(Guid versionId)
    {
        var version = await _versionWarehouse.GetOneAsync(versionId);
        var fileMetadata = version.Fields.FileMetadata;

        if (fileMetadata is null)
            throw new InvalidFileNameException("No document is defined for this provision.");

        var fileData = _fileStorage.GetFile(fileMetadata.NameInStorage);

        return new FileToStore(fileMetadata.Name, fileData);
    }
}