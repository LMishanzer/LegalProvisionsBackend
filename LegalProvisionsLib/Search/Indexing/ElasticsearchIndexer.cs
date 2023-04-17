using LegalProvisionsLib.Settings;
using Nest;

namespace LegalProvisionsLib.Search.Indexing;

public class ElasticsearchIndexer : IIndexer
{
    private readonly ElasticClient _client;
    
    public ElasticsearchIndexer(ElasticSettings settings)
    {
        var connectionSettings = new ConnectionSettings(new Uri(settings.Url)).DefaultIndex(settings.DefaultIndex);
        _client = new ElasticClient(connectionSettings);
    }
    
    public async Task IndexRecordAsync(IndexRecord record)
    {
        await _client.IndexDocumentAsync(record);
    }

    public async Task<IEnumerable<IndexRecord>> GetByRequestAsync(QueryModel query)
    {
        ISearchResponse<IndexRecord>? result = null;
        if (query.Keyword != null)
        {
            result = await GetByKeywordAsync(query.Keyword);
        }
        else if (query.ProvisionId.HasValue)
        {
            result = await GetByProvisionIdAsync(query.ProvisionId.Value);
        }

        return result?.Documents ?? Array.Empty<IndexRecord>();
    }

    public async Task DeleteRecordAsync(Guid provisionId)
    {
        await _client.DeleteByQueryAsync<IndexRecord>(r => r
            .Query(q => q
                .Match(m => m
                    .Field(f => f.ProvisionId).Query(provisionId.ToString()))));
    }

    private Task<ISearchResponse<IndexRecord>> GetByKeywordAsync(string keyword)
    {
        return _client.SearchAsync<IndexRecord>(r => r
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Keyword).Query(keyword))));
    }
    
    private Task<ISearchResponse<IndexRecord>> GetByProvisionIdAsync(Guid provisionId)
    {
        return _client.SearchAsync<IndexRecord>(r => r
            .Query(q => q
                .Match(m => m
                    .Field(f => f.ProvisionId).Query(provisionId.ToString()))));
    }
}