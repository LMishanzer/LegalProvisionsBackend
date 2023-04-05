using LegalProvisionsLib.DataPersistence.Models;

namespace LegalProvisionsLib.Validators;

public interface IProvisionValidator
{
    bool Validate(LegalProvision provision);
}