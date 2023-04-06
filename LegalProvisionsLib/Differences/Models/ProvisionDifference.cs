namespace LegalProvisionsLib.Differences.Models;

public class ProvisionDifference
{
    public Guid OriginalVersionId { get; set; }
    public Guid ChangedVersionId { get; set; }

    public List<Guid> RemovedContent { get; set; } = new();
    public List<Guid> AddedContent { get; set; } = new();
    public List<Guid> ChangedContent { get; set; } = new();
}