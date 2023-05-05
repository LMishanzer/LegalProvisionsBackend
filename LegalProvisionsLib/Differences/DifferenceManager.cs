using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.Differences.DifferenceCalculator;
using LegalProvisionsLib.Differences.Models;
using LegalProvisionsLib.Helpers;

namespace LegalProvisionsLib.Differences;

public class DifferenceManager : IDifferenceManager
{
    private readonly ProvisionVersionPersistence _provisionVersionPersistence;
    private readonly IDifferenceCalculator _differenceCalculator;

    public DifferenceManager(ProvisionVersionPersistence provisionVersionPersistence, IDifferenceCalculator differenceCalculator)
    {
        _provisionVersionPersistence = provisionVersionPersistence;
        _differenceCalculator = differenceCalculator;
    }
    
    public async Task<ProvisionDifference> GetVersionDifferencesAsync(Guid originalVersionId, Guid changedVersionId)
    {
        var original = await _provisionVersionPersistence.GetProvisionVersionAsync(originalVersionId);
        var changed = await _provisionVersionPersistence.GetProvisionVersionAsync(changedVersionId);

        return _differenceCalculator.CalculateDifferences(original, changed);
    }

    public async Task<ProvisionDifference> GetVersionDifferencesAsync(DifferenceRequest differenceRequest)
    {
        var firstDate = DateHelper.DateTimeToDate(differenceRequest.FirstProvisionIssue);
        var secondDate = DateHelper.DateTimeToDate(differenceRequest.SecondProvisionIssue);

        var firstProvision = await _provisionVersionPersistence.GetProvisionVersionAsync(differenceRequest.ProvisionId, firstDate);
        var secondProvision = await _provisionVersionPersistence.GetProvisionVersionAsync(differenceRequest.ProvisionId, secondDate);

        return _differenceCalculator.CalculateDifferences(firstProvision, secondProvision);
    }
}