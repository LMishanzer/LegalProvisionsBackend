namespace LegalProvisionsLib.Search.Indexing.Fulltext;

public interface IFulltextIndexer : IIndexer<FulltextRecord>
{
    Task DeleteByVersion(Guid versionId);
}