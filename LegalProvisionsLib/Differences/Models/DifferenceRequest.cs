// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace LegalProvisionsLib.Differences.Models;

public class DifferenceRequest
{
    public Guid ProvisionId { get; set; }
    public DateTime FirstProvisionIssue { get; set; }
    public DateTime SecondProvisionIssue { get; set; }
}