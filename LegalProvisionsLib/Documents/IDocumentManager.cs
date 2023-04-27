using LegalProvisionsLib.Documents.Models;
using LegalProvisionsLib.FileStorage.Models;

namespace LegalProvisionsLib.Documents;

public interface IDocumentManager
{
    Task AddProvisionDocumentAsync(Document document);
    Task<FileToStore> GetProvisionDocumentAsync(Guid versionId);
}