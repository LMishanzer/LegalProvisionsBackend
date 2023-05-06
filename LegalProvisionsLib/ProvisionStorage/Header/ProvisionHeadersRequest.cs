namespace LegalProvisionsLib.ProvisionStorage.Header;

public class ProvisionHeadersRequest
{
    public IEnumerable<Guid> ProvisionIds { get; set; } = Array.Empty<Guid>();
}