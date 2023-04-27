using LegalProvisionsLib.FileStorage.Models;

namespace LegalProvisionsLib.Documents.Models;

public class Document
{
    public Guid VersionId { get; set; }
    public FileToStore File { get; set; }
    
    public Document(Guid versionId, FileToStore file)
    {
        VersionId = versionId;
        File = file;
    }
}