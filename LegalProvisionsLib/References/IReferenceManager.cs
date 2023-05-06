namespace LegalProvisionsLib.References;

public interface IReferenceManager
{
    Task RemoveByHeaderIdAsync(Guid provisionHeaderId);
}