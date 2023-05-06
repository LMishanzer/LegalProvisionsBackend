using LegalProvisionsLib.Search.Indexing.FulltextIndexing;
using LegalProvisionsLib.Search.Indexing.KeywordsIndexing;
using Moq;

namespace LegalProvisionsLibTest.Search;

public class SearchHandlerTest
{
    private readonly Mock<IKeywordsIndexer> _keywordsIndexer = new();
    private readonly Mock<IFulltextIndexer> _fulltextIndexer = new();
    // private readonly Mock<IProvisionHandler> _provisionHandler = new();

    private const string SearchCase1 = "some text";
        
    [SetUp]
    public void Setup()
    {
        var firstId = Guid.NewGuid(); 
        var secondId = Guid.NewGuid(); 
        var thirdId = Guid.NewGuid();
        var fourthId = Guid.NewGuid();
        
        SetupKeywordsIndexer(firstId, secondId);
        _fulltextIndexer.Setup(k => k
                .GetByKeywordsAsync(SearchCase1))
            .Returns(() => new List<KeywordsRecord>
            {
                new()
                {
                    ProvisionId = firstId,
                    Text = "some text 1"
                },
                new()
                {
                    ProvisionId = secondId,
                    Text = "some text 2"
                }
            });
        
    }

    private void SetupKeywordsIndexer(Guid firstId, Guid secondId)
    {
        _keywordsIndexer.Setup(k => k
                .GetByKeywordsAsync(SearchCase1))
            .Returns(() => new List<KeywordsRecord>
            {
                new()
                {
                    ProvisionId = firstId,
                    Text = "some text 1"
                },
                new()
                {
                    ProvisionId = secondId,
                    Text = "some text 2"
                }
            });
    }
}