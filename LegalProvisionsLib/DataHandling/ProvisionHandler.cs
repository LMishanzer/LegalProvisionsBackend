using LegalProvisionsLib.DataHandling.Models;
using LegalProvisionsLib.DataPersistence;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Exceptions;
using LegalProvisionsLib.FileStorage;
using LegalProvisionsLib.Helpers;
using LegalProvisionsLib.Search.Indexing.FulltextIndexing;
using LegalProvisionsLib.Search.Indexing.KeywordsIndexing;
using LegalProvisionsLib.TextExtracting;

namespace LegalProvisionsLib.DataHandling;

public class ProvisionHandler : IProvisionHandler
{
    private readonly ProvisionVersionPersistence _provisionVersionPersistence;
    private readonly ProvisionHeaderPersistence _provisionHeaderPersistence;
    private readonly IKeywordsIndexer _keywordsIndexer;
    private readonly IFulltextIndexer _fulltextIndexer;
    private readonly IFileStorage _fileStorage;

    public ProvisionHandler(
        ProvisionVersionPersistence provisionVersionPersistence,
        ProvisionHeaderPersistence provisionHeaderPersistence,
        IKeywordsIndexer keywordsIndexer,
        IFulltextIndexer fulltextIndexer,
        IFileStorage fileStorage)
    {
        _provisionVersionPersistence = provisionVersionPersistence;
        _provisionHeaderPersistence = provisionHeaderPersistence;
        _keywordsIndexer = keywordsIndexer;
        _fulltextIndexer = fulltextIndexer;
        _fileStorage = fileStorage;
    }
    
    public async Task<Guid> AddProvisionAsync(ProvisionHeaderFields headerFields)
    {
        var provisionId = await _provisionHeaderPersistence.AddProvisionHeaderAsync(headerFields);
        
        await IndexKeywordsAndTitleAsync(headerFields, provisionId);

        return provisionId;
    }

    private async Task IndexKeywordsAndTitleAsync(ProvisionHeaderFields headerFields, Guid provisionId)
    {
        await _keywordsIndexer.IndexRecordAsync(new KeywordsRecord
        {
            Text = headerFields.Title,
            ProvisionId = provisionId
        });
        
        foreach (var keyword in headerFields.Keywords)
        {
            await _keywordsIndexer.IndexRecordAsync(new KeywordsRecord
            {
                Text = keyword,
                ProvisionId = provisionId
            });
        }
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
        
        var newVersionId = await _provisionVersionPersistence.AddProvisionAsync(versionFields);
        header.Fields.DatesOfChange.Add(versionFields.IssueDate);
        header.Fields.DatesOfChange = header.Fields.DatesOfChange.Order().ToList();

        await _provisionHeaderPersistence.UpdateProvisionHeaderAsync(headerId, header.Fields);

        await IndexFullTextAsync(provisionVersionFields: versionFields, provisionId: headerId, versionId: newVersionId);

        return newVersionId;
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
        return await _provisionVersionPersistence.GetProvisionVersionAsync(versionId);
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

    public async Task UpdateVersionAsync(Guid versionId, ProvisionVersionFields versionFields)
    {
        await _provisionVersionPersistence.UpdateVersionAsync(versionId, versionFields);
        
        // reindex the version
        await _fulltextIndexer.DeleteByVersion(versionId);
        await IndexFullTextAsync(versionFields, versionFields.ProvisionHeader, versionId);
    }

    public async Task UpdateHeaderAsync(Guid headerId, ProvisionHeaderFields headerFields)
    {
        await _provisionHeaderPersistence.UpdateProvisionHeaderAsync(headerId, headerFields);

        // reindex keywords and title
        await _keywordsIndexer.DeleteByProvisionAsync(headerId);
        await IndexKeywordsAndTitleAsync(headerFields, headerId);
    }

    public async Task DeleteProvisionAsync(Guid headerId)
    {
        var versions = await _provisionVersionPersistence.GetVersionsByHeaderIdAsync(headerId);

        var documents = versions
            .Select(v => v.Fields.FileMetadata)
            .Where(m => m is not null);

        foreach (var document in documents)
        {
            _fileStorage.DeleteFile(document!.NameInStorage);
        }
        
        await _provisionVersionPersistence.DeleteVersionsByHeaderAsync(headerId);
        await _provisionHeaderPersistence.DeleteProvisionHeaderAsync(headerId);

        await _keywordsIndexer.DeleteByProvisionAsync(headerId);
        await _fulltextIndexer.DeleteByProvisionAsync(headerId);
    }

    public async Task DeleteProvisionVersionAsync(Guid versionId)
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

        await _fulltextIndexer.DeleteByVersion(versionId);
    }

    private async Task IndexFullTextAsync(ITextExtractable provisionVersionFields, Guid provisionId, Guid versionId)
    {
        var entireText = string.Join(" ", provisionVersionFields.ExtractEntireText());
        
        await _fulltextIndexer.IndexRecordAsync(new FulltextRecord
        {
            Text = entireText,
            ProvisionId = provisionId,
            VersionId = versionId
        });
    }
}