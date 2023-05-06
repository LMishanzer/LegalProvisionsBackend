using LegalProvisionsLib.Search.Indexing;
using LegalProvisionsLib.Search.Indexing.Fulltext;
using LegalProvisionsLib.Search.Indexing.Keywords;
using LegalProvisionsLib.Search.SearchResultHandling;
using Moq;

namespace LegalProvisionsLibTest.Search.SearchResultsHandling;

public class SearchResultHandlerTest
{
    private readonly Mock<IKeywordsIndexer> _keywordIndexerMock = new();
    private readonly Mock<IFulltextIndexer> _fulltextIndexerMock = new();

    private const string SearchText = "some text";

    private List<KeywordsRecord> _keywordsResult = new();
    private List<FulltextRecord> _fulltextResult = new();

    [SetUp]
    public void Setup()
    {
        var keywordRecord1 = GetKeywordRecord();
        var keywordRecord2 = GetFulltextRecord();
        var keywordRecord3 = GetKeywordRecord();
        var keywordRecord4 = GetFulltextRecord(keywordRecord3.ProvisionId);

        _keywordsResult = new List<KeywordsRecord>
        {
            keywordRecord1, keywordRecord3
        };
        _fulltextResult = new List<FulltextRecord>
        {
            keywordRecord2, keywordRecord4
        };
        
        SetupKeywordIndexer(_keywordsResult);
        SetupFulltextIndexer(_fulltextResult);
    }

    [Test]
    public void Test1()
    {
        // Arrange
        var searchResultHandler = new SearchResultHandler(_keywordIndexerMock.Object, _fulltextIndexerMock.Object);
        var keywordsResultAsRecordList = _keywordsResult as IEnumerable<IRecord>;
        var fulltextResultAsRecordList = _fulltextResult as IEnumerable<IRecord>;
        var expectedOutput = 
            keywordsResultAsRecordList.UnionBy(fulltextResultAsRecordList, r => r.ProvisionId).ToArray();

        // Act
        var searchResultEnumerable = searchResultHandler.GetSearchResultsAsync(SearchText).Result;
        var searchResult = searchResultEnumerable.ToArray();

        // Assert
        Assert.That(searchResult, Has.Length.EqualTo(expectedOutput.Length));

        for (int i = 0; i < searchResult.Length; i++)
        {
            Assert.That(searchResult[i], Is.EqualTo(expectedOutput[i]));
        }
    }

    private void SetupKeywordIndexer(IEnumerable<KeywordsRecord> results)
    {
        _keywordIndexerMock.Setup(i => i.GetByKeywordsAsync(SearchText))
            .Returns(Task.FromResult(results));
    }
    
    private void SetupFulltextIndexer(IEnumerable<FulltextRecord> results)
    {
        _fulltextIndexerMock.Setup(i => i.GetByKeywordsAsync(SearchText))
            .Returns(Task.FromResult(results));
    }
    
    private static KeywordsRecord GetKeywordRecord(Guid? provisionId = null)
    {
        return new KeywordsRecord
        {
            ProvisionId = provisionId ?? Guid.NewGuid(),
            Text = SearchText
        };
    }
    
    private static FulltextRecord GetFulltextRecord(Guid? provisionId = null)
    {
        return new FulltextRecord
        {
            ProvisionId = provisionId ?? Guid.NewGuid(),
            Text = SearchText,
            VersionId = Guid.Empty
        };
    }
}