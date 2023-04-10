using LegalProvisionsLib.DataPersistence.Exceptions;
using LegalProvisionsLib.DataPersistence.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LegalProvisionsLib.DataPersistence;

public class ProvisionHeaderPersistence : DataPersistence
{
    private readonly IMongoCollection<ProvisionHeader> _headerCollection;

    public ProvisionHeaderPersistence(IMongoDatabase database)
    {
        _headerCollection = database.GetCollection<ProvisionHeader>("ProvisionHeaders");
    }
    
    public async Task<IEnumerable<ProvisionHeader>> GetAllProvisionHeadersAsync()
    {
        return await _headerCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task<ProvisionHeader> GetProvisionHeaderAsync(Guid id)
    {
        var foundHeaders = await _headerCollection.Find(GetFilterById(id)).ToListAsync();

        if (foundHeaders.Count != 1)
            throw new ElementsCountException("Wrong amount of ProvisionHeaders was found.");

        return foundHeaders.First();
    }
    
    public async Task<Guid> AddProvisionHeaderAsync(ProvisionHeaderFields fields)
    {
        var provisionHeader = new ProvisionHeader(fields);
        await _headerCollection.InsertOneAsync(provisionHeader);

        return provisionHeader.Id;
    }

    public async Task UpdateProvisionHeaderAsync(Guid id, ProvisionHeaderFields newHeaderFields)
    {
        var oldHeader = await GetProvisionHeaderAsync(id);

        oldHeader.Fields = newHeaderFields;

        await _headerCollection.ReplaceOneAsync(GetFilterById(id), oldHeader);
    }

    public async Task DeleteProvisionHeaderAsync(Guid headerId)
    {
        await _headerCollection.DeleteOneAsync(Builders<ProvisionHeader>.Filter.Eq(header => header.Id, headerId));
    }
}