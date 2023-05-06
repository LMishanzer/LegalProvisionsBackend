using LegalProvisionsLib.Settings;
using Nest;

namespace LegalProvisionsLib.Search.Indexing.Fulltext;

public class FulltextIndexer : Indexer<FulltextRecord>, IFulltextIndexer
{
    protected override ElasticClient Client { get; }

    public FulltextIndexer(ElasticSettings settings)
    {
        var connectionSettings = new ConnectionSettings(new Uri(settings.Url)).DefaultIndex(settings.FulltextIndex);
        Client = new ElasticClient(connectionSettings);
    }

    public async Task DeleteByVersion(Guid versionId)
    {
        await Client.DeleteByQueryAsync<FulltextRecord>(r => r
            .Query(q => q
                .Match(m => m
                    .Field(f => f.VersionId).Query(versionId.ToString()))));
    }
}