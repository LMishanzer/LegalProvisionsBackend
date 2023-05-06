namespace LegalProvisionsLib.ProvisionWarehouse.Header;

public class ProvisionHeadersRequest
{
    public IEnumerable<Guid> ProvisionIds { get; set; } = Array.Empty<Guid>();
}