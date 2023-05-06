using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.ProvisionWarehouse.Header;
using LegalProvisionsLib.Search;
using LegalProvisionsLib.Search.Indexing;
using LegalProvisionsLib.Search.Indexing.Keywords;
using LegalProvisionsLib.Search.SearchResultHandling;
using Moq;

namespace LegalProvisionsLibTest.Search;

public class SearchHandlerTest
{
    private readonly Mock<ISearchResultHandler> _searchResultHandler = new();
    private readonly Mock<IHeaderWarehouse> _headerStorage = new();

    private const string SearchCase1 = "some text";

    private readonly List<ProvisionHeader> _result = new();

    [SetUp]
    public void Setup()
    {
        var firstRecord = GetNewRecord();
        var secondRecord = GetNewRecord();
        
        SetupSearchResultHandler(firstRecord, secondRecord);
        SetupHeaderStorage(firstRecord, secondRecord);
    }

    [Test]
    public async Task TestCase()
    {
        // Arrange
        var searchHandler = new SearchHandler(_searchResultHandler.Object, _headerStorage.Object);
        
        // Act
        var result = searchHandler.SearchProvisionsAsync(SearchCase1);
        
        // Assert
        await foreach (var searchResult in result)
        {
            Assert.That(_result, Does.Contain(searchResult.ProvisionHeader));
            _result.Remove(searchResult.ProvisionHeader);
        }
        
        Assert.That(_result, Is.Empty);
    }

    private static KeywordsRecord GetNewRecord()
    {
        return new KeywordsRecord
        {
            ProvisionId = Guid.NewGuid(),
            Text = SearchCase1
        };
    }

    private void SetupSearchResultHandler(KeywordsRecord firstRecord, KeywordsRecord secondRecord)
    {
        _searchResultHandler.Setup(k => k
                .GetSearchResultsAsync(SearchCase1))
            .Returns(() => Task.FromResult((IEnumerable<IRecord>) new List<KeywordsRecord>
            {
                firstRecord,
                secondRecord
            }));
    }

    private void SetupHeaderStorage(IRecord firstRecord, IRecord secondRecord)
    {
        SetupHeaderStorageResponse(firstRecord);
        SetupHeaderStorageResponse(secondRecord);
    }

    private void SetupHeaderStorageResponse(IRecord record)
    {
        _headerStorage.Setup(h => h
                .GetOneAsync(record.ProvisionId))
            .Returns(() =>
            {
                var header = new ProvisionHeader(new ProvisionHeaderFields())
                {
                    Id = record.ProvisionId
                };
                
                _result.Add(header);

                return Task.FromResult((DataItem<ProvisionHeaderFields>)header);
            });
    }
}