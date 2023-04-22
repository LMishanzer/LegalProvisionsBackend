namespace LegalProvisionsLib.DataHandling.Models;

public class ProvisionHeadersRequest
{
    public IEnumerable<Guid> ProvisionIds { get; set; } = Array.Empty<Guid>();
}