namespace LegalProvisionsLib.Search.Indexing.FulltextIndexing;

public interface IFulltextIndexer : IIndexer<FulltextRecord>
{
    Task DeleteByVersion(Guid versionId);
}