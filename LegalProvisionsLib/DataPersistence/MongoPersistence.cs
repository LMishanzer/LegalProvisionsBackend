using LegalProvisionsLib.DataPersistence.Exceptions;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LegalProvisionsLib.DataPersistence;

public class MongoPersistence : IDataPersistence
{
    private readonly IMongoCollection<LegalProvision> _provisionCollection;
    
    public MongoPersistence(MongoSettings mongoSettings)
    {
        var client = new MongoClient(mongoSettings.ConnectionUri);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        _provisionCollection = database.GetCollection<LegalProvision>(mongoSettings.CollectionName);
    }
    
    public async Task<IEnumerable<LegalProvision>> GetAllProvisionsAsync()
    {
        return await _provisionCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task<LegalProvision> GetProvisionAsync(Guid id)
    {
        var provisionList = await _provisionCollection.Find(GetFilter(id)).ToListAsync();

        return provisionList.Count switch
        {
            > 1 => throw new ElementsCountException("Filter returned more than one element. Only one element was expected."),
            0 => throw new ElementsCountException("Filter didn't return any element. One element was expected."),
            _ => provisionList.First()
        };
    }

    public async Task<Guid> AddProvisionAsync(LegalProvisionFields provisionFields)
    {
        var provision = new LegalProvision(provisionFields);
        await _provisionCollection.InsertOneAsync(provision);

        return provision.Id;
    }

    public async Task UpdateProvisionAsync(Guid id, LegalProvisionFields newProvisionFields)
    {
        var provision = await GetProvisionAsync(id);
        provision.Fields = newProvisionFields;

        var updateResult = await _provisionCollection.ReplaceOneAsync(GetFilter(id), provision);

        if (updateResult.ModifiedCount > 1)
            throw new NotImplementedException();
    }

    public async Task DeleteProvisionAsync(Guid id)
    {
        await GetProvisionAsync(id);

        var deleteResult = await _provisionCollection.DeleteOneAsync(GetFilter(id));
        
        if (deleteResult.DeletedCount > 1)
            throw new NotImplementedException();
    }

    public async Task DeleteAllProvisionsAsync() => await _provisionCollection.DeleteManyAsync(new BsonDocument());

    private static BsonDocument GetFilter(Guid id)
    {
        return new BsonDocument(
            new BsonElement("_id", id)
        );
    }
}