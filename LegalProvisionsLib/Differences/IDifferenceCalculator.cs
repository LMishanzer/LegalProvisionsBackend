using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences.Models;

namespace LegalProvisionsLib.Differences;

public interface IDifferenceCalculator
{
    ProvisionDifference CalculateDifferences(ProvisionVersion original, ProvisionVersion changed);
}