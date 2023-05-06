namespace LegalProvisionsLib.DataHandling.Header;

public class ProvisionHeadersRequest
{
    public IEnumerable<Guid> ProvisionIds { get; set; } = Array.Empty<Guid>();
}