using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Exceptions;
using LegalProvisionsLib.Helpers;

namespace LegalProvisionsLib.ProvisionStorage.DataHandling.Version;

public class VersionHandler : IVersionHandler
{
    private readonly ProvisionVersionPersistence _provisionVersionPersistence;
    private readonly ProvisionHeaderPersistence _provisionHeaderPersistence;

    public VersionHandler(
        ProvisionVersionPersistence provisionVersionPersistence,
        ProvisionHeaderPersistence provisionHeaderPersistence)
    {
        _provisionVersionPersistence = provisionVersionPersistence;
        _provisionHeaderPersistence = provisionHeaderPersistence;
    }
    
    public async Task<Guid> AddAsync(ProvisionVersionFields versionFields)
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
        
        var newVersionId = await _provisionVersionPersistence.AddProvisionAsync(versionFields);
        header.Fields.DatesOfChange.Add(versionFields.IssueDate);
        header.Fields.DatesOfChange = header.Fields.DatesOfChange.Order().ToList();

        await _provisionHeaderPersistence.UpdateProvisionHeaderAsync(headerId, header.Fields);

        return newVersionId;
    }

    public async Task<DataItem<ProvisionVersionFields>> GetOneAsync(Guid versionId)
    {
        return await _provisionVersionPersistence.GetProvisionVersionAsync(versionId);
    }

    public async Task UpdateAsync(Guid versionId, ProvisionVersionFields versionFields)
    {
        await _provisionVersionPersistence.UpdateVersionAsync(versionId, versionFields);
    }

    public async Task DeleteAsync(Guid versionId)
    {
        var version = await _provisionVersionPersistence.GetProvisionVersionAsync(versionId);
        var header = await _provisionHeaderPersistence.GetProvisionHeaderAsync(version.Fields.ProvisionHeader);
        var issueDate = version.Fields.IssueDate;
        header.Fields.DatesOfChange.Remove(issueDate);

        await _provisionVersionPersistence.DeleteProvisionAsync(versionId);
        
        if (header.Fields.DatesOfChange.Count == 0)
        {
            await _provisionHeaderPersistence.DeleteProvisionHeaderAsync(header.Id);
        }
        else
        {
            await _provisionHeaderPersistence.UpdateProvisionHeaderAsync(header.Id, header.Fields);
        }
    }

    public async Task<ProvisionVersion> GetActualProvisionVersionAsync(Guid versionId)
    {
        try
        {
            return await _provisionVersionPersistence.GetActualVersionAsync(versionId);
        }
        catch (ElementsCountException e)
        {
            throw new ClientSideException("Provision doesn't have any version", e);
        }
    }

    public async Task<ProvisionVersion> GetProvisionVersionAsync(Guid headerId, DateTime issueDate)
    {
        var issueDay = DateHelper.DateTimeToDate(issueDate);
        return await _provisionVersionPersistence.GetProvisionVersionAsync(headerId, issueDay);
    }
}