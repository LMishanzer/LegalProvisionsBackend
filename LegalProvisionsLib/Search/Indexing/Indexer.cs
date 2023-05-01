using Nest;

namespace LegalProvisionsLib.Search.Indexing;

public abstract class Indexer<T> : IIndexer<T> where T : class, IRecord
{
    protected abstract ElasticClient Client { get; }
    
    public async Task IndexRecordAsync(T record)
    {
        await Client.IndexDocumentAsync(record);
    }

    public async Task<IEnumerable<T>> GetByKeywordsAsync(string keyword)
    {
        var result = await Client.SearchAsync<T>(r => r
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Text).Query(keyword))));

        return result.Documents;
    }

    public async Task DeleteRecordAsync(Guid provisionId)
    {
        await Client.DeleteByQueryAsync<T>(r => r
            .Query(q => q
                .Match(m => m
                    .Field(f => f.ProvisionId).Query(provisionId.ToString()))));
    }
}