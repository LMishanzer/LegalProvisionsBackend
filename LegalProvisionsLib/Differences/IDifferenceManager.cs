using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.Differences.Models;

namespace LegalProvisionsLib.Differences;

public interface IDifferenceManager
{
    Task<ProvisionDifference> GetVersionDifferencesAsync(Guid original, Guid changed);
    Task<ProvisionDifference> GetVersionDifferencesAsync(DifferenceRequest differenceRequest);
}