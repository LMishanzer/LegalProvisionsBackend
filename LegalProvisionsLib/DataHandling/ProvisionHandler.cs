using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.DataPersistence.Exceptions;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences;
using LegalProvisionsLib.Differences.Models;
using LegalProvisionsLib.Exceptions;
using LegalProvisionsLib.Helpers;

namespace LegalProvisionsLib.DataHandling;

public class ProvisionHandler : IProvisionHandler
{
    private readonly MongoPersistence _mongoPersistence;
    private readonly IDifferenceCalculator _differenceCalculator;

    public ProvisionHandler(MongoPersistence mongoPersistence, IDifferenceCalculator differenceCalculator)
    {
        _mongoPersistence = mongoPersistence;
        _differenceCalculator = differenceCalculator;
    }
    
    public async Task<Guid> AddProvision(ProvisionHeaderFields headerFields)
    {
        return await _mongoPersistence.AddProvisionHeaderAsync(headerFields);
    }

    public async Task<Guid> AddProvisionVersion(ProvisionVersionFields versionFields)
    {
        var headerId = versionFields.ProvisionHeader;

        ProvisionHeader header;

        // check existence
        try
        {
            header = await _mongoPersistence.GetProvisionHeaderAsync(headerId);
        }
        catch(ElementsCountException e)
        {
            throw new ClientSideException("No such provision header", e);
        }
        
        var resultId = await _mongoPersistence.AddProvisionAsync(versionFields);
        header.Fields.DatesOfChange.Add(versionFields.IssueDate);
        header.Fields.DatesOfChange = header.Fields.DatesOfChange.Order().ToList();

        await _mongoPersistence.UpdateProvisionHeaderAsync(headerId, header.Fields);

        return resultId;
    }

    public async Task<IEnumerable<ProvisionHeader>> GetAllProvisions()
    {
        return await _mongoPersistence.GetAllProvisionHeadersAsync();
    }

    public async Task<ProvisionHeader> GetProvisionHeader(Guid id)
    {
        return await _mongoPersistence.GetProvisionHeaderAsync(id);
    }
    
    public async Task<ProvisionVersion> GetProvisionVersion(Guid id, DateTime issueDate)
    {
        var issueDay = DateHelper.DateTimeToDate(issueDate);
        return await _mongoPersistence.GetProvisionVersionAsync(id, issueDay);
    }

    public async Task<ProvisionVersion> GetActualProvisionVersion(Guid headerId)
    {
        try
        {
            return await _mongoPersistence.GetActualVersionAsync(headerId);
        }
        catch (ElementsCountException e)
        {
            throw new ClientSideException("Provision doesn't have any version", e);
        }
    }

    public async Task<ProvisionDifference> GetVersionDifferences(Guid originalVersionId, Guid changedVersionId)
    {
        var original = await _mongoPersistence.GetProvisionAsync(originalVersionId);
        var changed = await _mongoPersistence.GetProvisionAsync(changedVersionId);

        return _differenceCalculator.CalculateDifferences(original, changed);
    }

    public async Task<ProvisionDifference> GetVersionDifferences(DifferenceRequest differenceRequest)
    {
        var firstDate = DateHelper.DateTimeToDate(differenceRequest.FirstProvisionIssue);
        var secondDate = DateHelper.DateTimeToDate(differenceRequest.SecondProvisionIssue);

        var firstProvision = await _mongoPersistence.GetProvisionVersionAsync(differenceRequest.ProvisionId, firstDate);
        var secondProvision = await _mongoPersistence.GetProvisionVersionAsync(differenceRequest.ProvisionId, secondDate);

        return _differenceCalculator.CalculateDifferences(firstProvision, secondProvision);
    }
}