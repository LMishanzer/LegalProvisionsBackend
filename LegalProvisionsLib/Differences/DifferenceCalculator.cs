using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences.Models;

namespace LegalProvisionsLib.Differences;

public class DifferenceCalculator : IDifferenceCalculator
{
    public ProvisionDifference CalculateDifferences(ProvisionVersion original, ProvisionVersion changed)
    {
        var originalContent = original.Fields.GetAllContentDictionary().ToDictionary(content => content.Id);
        var changedContent = changed.Fields.GetAllContentDictionary().ToList().ToDictionary(content => content.Id);

        var originalContentIds = originalContent.Select(content => content.Key).ToHashSet();
        var changedContentIds = changedContent.Select(content => content.Key).ToHashSet();

        var removedContent = originalContentIds.Except(changedContentIds);
        var addedContent = changedContentIds.Except(originalContentIds);
        var possiblyChanged = originalContentIds.Intersect(changedContentIds);

        var difference = new ProvisionDifference
        {
            OriginalVersionId = original.Id,
            ChangedVersionId = changed.Id
        };

        foreach (var id in removedContent)
        {
            difference.RemovedContent.Add(id);
        }

        foreach (var id in addedContent)
        {
            difference.AddedContent.Add(id);
        }

        foreach (var id in possiblyChanged)
        {
            var originalItem = originalContent[id];
            var changedItem = changedContent[id];

            if (originalItem.Title != changedItem.Title || originalItem.TextMain != changedItem.TextMain)
            {
                difference.ChangedContent.Add(id);
            }
        }

        return difference;
    }
}