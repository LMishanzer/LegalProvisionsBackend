using LegalProvisionsLib.DataPersistence;

namespace LegalProvisionsLib.References;

public class ReferenceManager : IReferenceManager
{
    private readonly ProvisionVersionPersistence _provisionVersionPersistence;

    public ReferenceManager(ProvisionVersionPersistence provisionVersionPersistence)
    {
        _provisionVersionPersistence = provisionVersionPersistence;
    }
    
    public async Task RemoveByHeaderIdAsync(Guid provisionHeaderId) => 
        await _provisionVersionPersistence.RemoveReferencesToHeaderAsync(provisionHeaderId);
}