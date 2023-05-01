using LegalProvisionsLib.Settings;
using Nest;

namespace LegalProvisionsLib.Search.Indexing.FulltextIndexing;

public class FulltextIndexer : Indexer<FulltextRecord>
{
    protected override ElasticClient Client { get; }

    public FulltextIndexer(ElasticSettings settings)
    {
        var connectionSettings = new ConnectionSettings(new Uri(settings.Url)).DefaultIndex(settings.FulltextIndex);
        Client = new ElasticClient(connectionSettings);
    }
}