using LegalProvisionsLib.DataPersistence.Exceptions;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LegalProvisionsLib.DataPersistence;

public class MongoPersistence : IDataPersistence
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<ProvisionVersion> _provisionCollection;
    
    public MongoPersistence(MongoSettings mongoSettings)
    {
        var client = new MongoClient(mongoSettings.ConnectionUri);
        _mongoDatabase = client.GetDatabase(mongoSettings.DatabaseName);
        _provisionCollection = _mongoDatabase.GetCollection<ProvisionVersion>("ProvisionVersions");
    }

    #region ProvisionVersions

    public async Task<IEnumerable<ProvisionVersion>> GetAllProvisionsAsync()
    {
        return await _provisionCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<ProvisionVersion> GetProvisionAsync(Guid id)
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

    public async Task UpdateProvisionAsync(Guid id, ProvisionVersionFields newProvisionVersionFields)
    {
        var provision = await GetProvisionAsync(id);
        provision.Fields = newProvisionVersionFields;

        await _provisionCollection.ReplaceOneAsync(GetFilterById(id), provision);
    }

    public async Task DeleteProvisionAsync(Guid id)
    {
        await GetProvisionAsync(id);
        await _provisionCollection.DeleteOneAsync(GetFilterById(id));
    }

    public async Task DeleteAllProvisionsAsync() => await _provisionCollection.DeleteManyAsync(new BsonDocument());
 
    #endregion

    #region ProvisionHeaders

    public async Task<IEnumerable<ProvisionHeader>> GetAllProvisionHeadersAsync()
    {
        var provisionHeadersCollection = GetProvisionHeadersCollection();

        return await provisionHeadersCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task<ProvisionHeader> GetProvisionHeaderAsync(Guid id)
    {
        var provisionHeadersCollection = GetProvisionHeadersCollection();

        var foundHeaders = await provisionHeadersCollection.Find(GetFilterById(id)).ToListAsync();

        if (foundHeaders.Count != 1)
            throw new ElementsCountException("Wrong amount of ProvisionHeaders was found.");

        return foundHeaders.First();
    }
    
    public async Task<Guid> AddProvisionHeaderAsync(ProvisionHeaderFields fields)
    {
        var provisionHeader = new ProvisionHeader(fields);
        var provisionHeadersCollection = GetProvisionHeadersCollection();

        await provisionHeadersCollection.InsertOneAsync(provisionHeader);

        return provisionHeader.Id;
    }

    public async Task UpdateProvisionHeaderAsync(Guid id, ProvisionHeaderFields newHeaderFields)
    {
        var collection = GetProvisionHeadersCollection();
        var oldHeader = await GetProvisionHeaderAsync(id);

        oldHeader.Fields = newHeaderFields;

        await collection.ReplaceOneAsync(GetFilterById(id), oldHeader);
    }
    
    private IMongoCollection<ProvisionHeader> GetProvisionHeadersCollection() => _mongoDatabase.GetCollection<ProvisionHeader>("ProvisionHeaders");

    #endregion

    private static BsonDocument GetFilterById(Guid id)
    {
        return new BsonDocument(
            new BsonElement("_id", id)
        );
    }
}