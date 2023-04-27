using MongoDB.Bson;

namespace LegalProvisionsLib.DataPersistence;

public abstract class DataPersistence
{
    protected static BsonDocument GetFilterById(Guid id)
    {
        return new BsonDocument(
            new BsonElement("_id",  new BsonBinaryData(id, GuidRepresentation.Standard))
        );
    }
}