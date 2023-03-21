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

    public async Task<LegalProvision> GetProvisionAsync(string id)
    {
        var provisionList = await _provisionCollection.Find(
            new BsonDocument(
                new BsonElement("_id", new BsonObjectId(new ObjectId(id)))
            )
        ).ToListAsync();

        return provisionList.Count switch
        {
            > 1 => throw new ElementsCountException("Filter returned more than one element. Only one element was expected."),
            0 => throw new ElementsCountException("Filter didn't return any element. One element was expected."),
            _ => provisionList.First()
        };
    }

    public async Task<string> AddProvisionAsync(LegalProvision provision)
    {
        provision.Id = ObjectId.GenerateNewId();
        await _provisionCollection.InsertOneAsync(provision);

        return provision.Id.ToString();
    }

    public async Task UpdateProvisionAsync(string id, LegalProvisionUpdate newProvision)
    {
        var provision = await GetProvisionAsync(id);

        var filter = new BsonDocument(
            new BsonElement("_id", new BsonObjectId(new ObjectId(id)))
        );

        provision.Title = newProvision.Title;
        provision.Articles = newProvision.Articles;

        var updateResult = await _provisionCollection.ReplaceOneAsync(filter, provision);

        if (updateResult.ModifiedCount > 1)
            throw new NotImplementedException();
    }

    public async Task DeleteProvisionAsync(string id)
    {
        await GetProvisionAsync(id);

        var deleteResult = await _provisionCollection.DeleteOneAsync(new BsonDocument(
            new BsonElement("_id", new BsonObjectId(new ObjectId(id)))
        ));
        
        if (deleteResult.DeletedCount > 1)
            throw new NotImplementedException();
    }
}