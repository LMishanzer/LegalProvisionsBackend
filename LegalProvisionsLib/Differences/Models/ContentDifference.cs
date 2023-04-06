namespace LegalProvisionsLib.Differences.Models;

public class ContentDifference
{
    public Guid ContentId { get; set; }
    public ContentChange Change { get; set; }
}