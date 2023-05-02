namespace LegalProvisionsLib.Search.Indexing;

public interface IIndexer<T>
{
    Task IndexRecordAsync(T record);

    Task<IEnumerable<T>> GetByKeywordsAsync(string keyword);

    Task DeleteByProvisionAsync(Guid provisionId);
}