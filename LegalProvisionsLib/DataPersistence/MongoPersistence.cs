using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LegalProvisionsLib.DataPersistence;

public class MongoPersistence : IDataPersistence
{
    private readonly IMongoCollection<LegalProvision> _provisionCollection;
    
    public MongoPersistence()
    {
        var mongoSettings = MongoSettings.Instance;
        var client = new MongoClient(mongoSettings.ConnectionUri);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        _provisionCollection = database.GetCollection<LegalProvision>(mongoSettings.CollectionName);
    }
    
    public async Task<IEnumerable<LegalProvision>> GetAllProvisionsAsync()
    {
        return await _provisionCollection.Find(new BsonDocument()).ToListAsync();
    }

    public Task<LegalProvision> GetProvisionAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddProvisionAsync(LegalProvision provision)
    {
        await _provisionCollection.InsertOneAsync(provision);

        return "Here should be an ID.";
    }

    public Task UpdateProvisionAsync(LegalProvision provision)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProvisionAsync(string id)
    {
        throw new NotImplementedException();
    }
}