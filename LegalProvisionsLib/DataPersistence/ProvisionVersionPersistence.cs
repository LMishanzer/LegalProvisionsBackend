﻿using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Exceptions;
using MongoDB.Driver;

namespace LegalProvisionsLib.DataPersistence;

public class ProvisionVersionPersistence : DataPersistence
{
    private readonly IMongoCollection<ProvisionVersion> _provisionCollection;
    
    public ProvisionVersionPersistence(IMongoDatabase database)
    {
        _provisionCollection = database.GetCollection<ProvisionVersion>("ProvisionVersions");
    }

    public async Task<ProvisionVersion> GetProvisionVersionAsync(Guid id)
    {
        var provisionList = await _provisionCollection.Find(GetFilterById(id)).ToListAsync();

        return provisionList.Count switch
        {
            > 1 => throw new ElementsCountException("Filter returned more than one element. Only one element was expected."),
            0 => throw new ElementsCountException("Filter didn't return any element. One element was expected."),
            _ => provisionList.First()
        };
    }

    public async Task<ProvisionVersion> GetActualVersionAsync(Guid headerId)
    {
        var filter = Builders<ProvisionVersion>.Filter.Eq(version => version.Fields.ProvisionHeader, headerId);
        var sort = Builders<ProvisionVersion>.Sort.Descending(version => version.Fields.IssueDate);

        var provisionVersionList = await _provisionCollection.Find(filter).Sort(sort).Limit(1).ToListAsync();

        if (provisionVersionList.Count != 1)
            throw new ElementsCountException("");

        return provisionVersionList.First();
    }
    
    public async Task<ProvisionVersion> GetProvisionVersionAsync(Guid headerId, DateOnly issueDate)
    {
        var filter = Builders<ProvisionVersion>.Filter.Eq(version => version.Fields.ProvisionHeader, headerId)
            & Builders<ProvisionVersion>.Filter.Eq(version => version.Fields.IssueDate, issueDate);

        var provisionVersionList = await _provisionCollection.Find(filter).Limit(1).ToListAsync();

        if (provisionVersionList.Count != 1)
            throw new ElementsCountException("");

        return provisionVersionList.First();
    }

    public async Task<IEnumerable<ProvisionVersion>> GetVersionsByHeaderIdAsync(Guid headerId)
    {
        var filter = Builders<ProvisionVersion>.Filter.Eq(version => version.Fields.ProvisionHeader, headerId);

        return await _provisionCollection.Find(filter).ToListAsync();
    }

    public async Task<Guid> AddProvisionAsync(ProvisionVersionFields provisionVersionFields)
    {
        var provision = new ProvisionVersion(provisionVersionFields);
        await _provisionCollection.InsertOneAsync(provision);

        return provision.Id;
    }

    public async Task UpdateVersionAsync(Guid id, ProvisionVersionFields newProvisionVersionFields)
    {
        var provision = await GetProvisionVersionAsync(id);
        provision.Fields = newProvisionVersionFields;

        await _provisionCollection.ReplaceOneAsync(GetFilterById(id), provision);
    }

    public async Task DeleteProvisionAsync(Guid id)
    {
        await GetProvisionVersionAsync(id);
        await _provisionCollection.DeleteOneAsync(GetFilterById(id));
    }

    public async Task DeleteVersionsByHeaderAsync(Guid headerId)
    {
        var filter = Builders<ProvisionVersion>.Filter.Eq(version => version.Fields.ProvisionHeader, headerId);
        await _provisionCollection.DeleteManyAsync(filter);
    }

    public async Task RemoveReferencesToHeaderAsync(Guid headerId)
    {
        var filter = Builders<ProvisionVersion>.Filter.ElemMatch(x => x.Fields.Content!.References, 
            o => o.ProvisionId == headerId);
        var update = Builders<ProvisionVersion>.Update.PullFilter(x => 
            x.Fields.Content!.References, o => o.ProvisionId == headerId);
        
        await _provisionCollection.UpdateManyAsync(filter, update);
    }
}