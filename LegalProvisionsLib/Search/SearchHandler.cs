using LegalProvisionsLib.DataHandling;
using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Search.Indexing;
using LegalProvisionsLib.Search.Indexing.KeywordsIndexing;
using MongoDB.Driver.Linq;

namespace LegalProvisionsLib.Search;

public class SearchHandler : ISearchHandler
{
    private readonly IIndexer<KeywordsRecord> _indexer;
    private readonly IProvisionHandler _provisionHandler;

    public SearchHandler(IIndexer<KeywordsRecord> indexer, IProvisionHandler provisionHandler)
    {
        _indexer = indexer;
        _provisionHandler = provisionHandler;
    }
    
    public async Task<IEnumerable<ProvisionHeader>> SearchProvisionsAsync(string keywords)
    {
        var searchResult = await _indexer.GetByKeywordsAsync(keywords);

        var indexRecords = searchResult as KeywordsRecord[] ?? searchResult.ToArray();
        var taskList = new List<Task<ProvisionHeader?>>(indexRecords.Length);
        var provisionList = new List<ProvisionHeader>(indexRecords.Length);

        foreach (var indexRecord in indexRecords)
        {
            var currentTask = GetHeaderSafeAsync(indexRecord.ProvisionId); 
            taskList.Add(currentTask);
        }

        await Task.WhenAll(taskList);

        provisionList.AddRange(taskList.Where(task => task.Result != null).Select(task => task.Result!));

        return provisionList.DistinctBy(p => p.Id);
    }

    private async Task<ProvisionHeader?> GetHeaderSafeAsync(Guid provisionId)
    {
        try
        {
            return await _provisionHandler.GetProvisionHeaderAsync(provisionId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}