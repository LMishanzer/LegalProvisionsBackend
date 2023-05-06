using LegalProvisionsLib.Settings;
using Nest;

namespace LegalProvisionsLib.Search.Indexing.Keywords;

public class KeywordsIndexer : Indexer<KeywordsRecord>, IKeywordsIndexer
{
    protected override ElasticClient Client { get; }
    
    public KeywordsIndexer(ElasticSettings settings)
    {
        var connectionSettings = new ConnectionSettings(new Uri(settings.Url)).DefaultIndex(settings.DefaultIndex);
        Client = new ElasticClient(connectionSettings);
    }
}