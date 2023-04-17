namespace LegalProvisionsLib.Search.Indexing;

public interface IIndexer
{
    Task IndexRecordAsync(IndexRecord record);

    Task<IEnumerable<IndexRecord>> GetByRequestAsync(QueryModel query);

    Task DeleteRecordAsync(Guid provisionId);
}