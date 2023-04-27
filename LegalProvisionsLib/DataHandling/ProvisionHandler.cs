using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences;
using LegalProvisionsLib.Differences.Models;
using LegalProvisionsLib.Exceptions;
using LegalProvisionsLib.FileStorage;
using LegalProvisionsLib.Helpers;
using LegalProvisionsLib.Search.Indexing;

namespace LegalProvisionsLib.DataHandling;

public class ProvisionHandler : IProvisionHandler
{
    private readonly ProvisionVersionPersistence _provisionVersionPersistence;
    private readonly ProvisionHeaderPersistence _provisionHeaderPersistence;
    private readonly IIndexer _indexer;
    private readonly IFileStorage _fileStorage;
    private readonly IDifferenceCalculator _differenceCalculator;

    public ProvisionHandler(
        ProvisionVersionPersistence provisionVersionPersistence, 
        IDifferenceCalculator differenceCalculator, 
        ProvisionHeaderPersistence provisionHeaderPersistence,
        IIndexer indexer,
        IFileStorage fileStorage)
    {
        _provisionVersionPersistence = provisionVersionPersistence;
        _differenceCalculator = differenceCalculator;
        _provisionHeaderPersistence = provisionHeaderPersistence;
        _indexer = indexer;
        _fileStorage = fileStorage;
    }
    
    public async Task<Guid> AddProvisionAsync(ProvisionHeaderFields headerFields)
    {
        var id = await _provisionHeaderPersistence.AddProvisionHeaderAsync(headerFields);

        await _indexer.IndexRecordAsync(new IndexRecord
        {
            Keyword = headerFields.Title,
            ProvisionId = id
        });

        foreach (var keyword in headerFields.Keywords)
        {
            await _indexer.IndexRecordAsync(new IndexRecord
            {
                Keyword = keyword,
                ProvisionId = id
            });
        }

        return id;
    }

    public async Task<Guid> AddProvisionVersionAsync(ProvisionVersionFields versionFields)
    {
        var headerId = versionFields.ProvisionHeader;

        ProvisionHeader header;

        // check existence
        try
        {
            header = await _provisionHeaderPersistence.GetProvisionHeaderAsync(headerId);
        }
        catch(ElementsCountException e)
        {
            throw new ClientSideException("No such provision header", e);
        }
        
        var resultId = await _provisionVersionPersistence.AddProvisionAsync(versionFields);
        header.Fields.DatesOfChange.Add(versionFields.IssueDate);
        header.Fields.DatesOfChange = header.Fields.DatesOfChange.Order().ToList();

        await _provisionHeaderPersistence.UpdateProvisionHeaderAsync(headerId, header.Fields);

        return resultId;
    }

    public async Task<IEnumerable<ProvisionHeader>> GetAllProvisionsAsync()
    {
        return await _provisionHeaderPersistence.GetAllProvisionHeadersAsync();
    }

    public async Task<ProvisionHeader> GetProvisionHeaderAsync(Guid id)
    {
        return await _provisionHeaderPersistence.GetProvisionHeaderAsync(id);
    }

    public async Task<IEnumerable<ProvisionHeader>> GetProvisionHeadersAsync(ProvisionHeadersRequest request)
    {
        return await _provisionHeaderPersistence.GetProvisionHeadersAsync(request.ProvisionIds);
    }

    public async Task<ProvisionVersion> GetProvisionVersionAsync(Guid id, DateTime issueDate)
    {
        var issueDay = DateHelper.DateTimeToDate(issueDate);
        return await _provisionVersionPersistence.GetProvisionVersionAsync(id, issueDay);
    }
    
    public async Task<ProvisionVersion> GetProvisionVersionAsync(Guid versionId)
    {
        return await _provisionVersionPersistence.GetProvisionAsync(versionId);
    }

    public async Task<ProvisionVersion> GetActualProvisionVersionAsync(Guid headerId)
    {
        try
        {
            return await _provisionVersionPersistence.GetActualVersionAsync(headerId);
        }
        catch (ElementsCountException e)
        {
            throw new ClientSideException("Provision doesn't have any version", e);
        }
    }

    public async Task<ProvisionDifference> GetVersionDifferencesAsync(Guid originalVersionId, Guid changedVersionId)
    {
        var original = await _provisionVersionPersistence.GetProvisionAsync(originalVersionId);
        var changed = await _provisionVersionPersistence.GetProvisionAsync(changedVersionId);

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

    public async Task UpdateVersionAsync(Guid versionId, ProvisionVersionFields versionFields)
    {
        await _provisionVersionPersistence.UpdateVersionAsync(versionId, versionFields);
    }

    public async Task DeleteProvisionAsync(Guid headerId)
    {
        var versions = await _provisionVersionPersistence.GetVersionsByHeaderIdAsync(headerId);

        var documents = versions
            .Select(v => v.Fields.FileMetadata)
            .Where(m => m is not null);
        
        await _provisionVersionPersistence.DeleteVersionsByHeaderAsync(headerId);
        await _provisionHeaderPersistence.DeleteProvisionHeaderAsync(headerId);

        await _indexer.DeleteRecordAsync(headerId);
    }

    public async Task DeleteProvisionVersionAsync(Guid versionId)
    {
        var version = await _provisionVersionPersistence.GetProvisionAsync(versionId);
        var header = await _provisionHeaderPersistence.GetProvisionHeaderAsync(version.Fields.ProvisionHeader);
        var issueDate = version.Fields.IssueDate;
        header.Fields.DatesOfChange.Remove(issueDate);
        await _provisionVersionPersistence.DeleteProvisionAsync(versionId);
        await _provisionHeaderPersistence.UpdateProvisionHeaderAsync(header.Id, header.Fields);

        if (version.Fields.FileMetadata is not null)
        {
            _fileStorage.DeleteFile(version.Fields.FileMetadata.NameInStorage);
        }
    }
}