using LegalProvisionsLib.DataPersistence.Models;
using LegalProvisionsLib.Differences.DifferenceCalculator;
using LegalProvisionsLibTest.Utilities;

namespace LegalProvisionsLibTest.Differences;

public class DifferenceCalculatorTest
{
    private readonly IDifferenceCalculator _differenceCalculator = new DifferenceCalculator();

    [Test]
    public void IdentityProvisionsTest()
    {
        var fields = new ProvisionVersionFields
        {
            Content = new ContentItem
            {
                Title = "Test",
                TextMain = "some main text",
                InnerItems = new[]
                {
                    new ContentItem
                    {
                        Identifier = "1a",
                        Title = "some title",
                        TextMain = "some child text"
                    }
                }
            }
        };
        var original = new ProvisionVersion(fields);
        var same = new ProvisionVersion(fields);
        
        var difference = _differenceCalculator.CalculateDifferences(original: original, changed: same);
        Assert.Multiple(() =>
        {
            Assert.That(difference.AddedContent, Is.Empty);
            Assert.That(difference.RemovedContent, Is.Empty);
            Assert.That(difference.ChangedContent, Is.Empty);
            Assert.That(difference.ChangedVersionId, Is.Not.EqualTo(difference.OriginalVersionId));
        });
    }
    
    [Test]
    public void ChangeTest1()
    {
        var fieldsOriginal = new ProvisionVersionFields
        {
            Content = new ContentItem
            {
                Title = "Test",
                TextMain = "some main text",
                InnerItems = new[]
                {
                    new ContentItem
                    {
                        Identifier = "1a",
                        Title = "some title",
                        TextMain = "some child text"
                    }
                }
            }
        };
        
        var fieldsChanged = DeepCopyHelper.DeepCopy(fieldsOriginal);

        fieldsChanged!.Content!.TextMain = "changed text";
        var changedItem = fieldsChanged.Content;
        var items = fieldsChanged.Content.InnerItems.ToList();
        var addedItem = new ContentItem
        {
            Identifier = "id",
            Title = "Second part",
            TextMain = "Text of the second part"
        };
        items.Add(addedItem);
        fieldsChanged.Content.InnerItems = items;

        var original = new ProvisionVersion(fieldsOriginal);
        var changed = new ProvisionVersion(fieldsChanged);

        var difference = _differenceCalculator.CalculateDifferences(original, changed);
        
        Assert.Multiple(() =>
        {
            Assert.That(difference.AddedContent, Has.Count.EqualTo(1));
            Assert.That(difference.AddedContent.First(), Is.EqualTo(addedItem.Id));
            Assert.That(difference.ChangedContent, Has.Count.EqualTo(1));
            Assert.That(difference.ChangedContent.First(), Is.EqualTo(changedItem.Id));
        });
    }

    [Test]
    public void ChangeTest2()
    {
        var fieldsOriginal = new ProvisionVersionFields
        {
            Content = new ContentItem
            {
                Title = "Test",
                TextMain = "some main text",
                InnerItems = new[]
                {
                    new ContentItem
                    {
                        Identifier = "1a",
                        Title = "some title",
                        TextMain = "some child text"
                    }
                }
            }
        };

        var fieldsChanged = DeepCopyHelper.DeepCopy(fieldsOriginal);

        fieldsChanged!.Content!.TextMain = "changed text";
        var items = fieldsChanged.Content.InnerItems.ToList();
        var deletedItem = items.First();
        var addedItem = new ContentItem
        {
            Identifier = "id",
            Title = "Second part",
            TextMain = "Text of the second part"
        };
        items = new List<ContentItem>{addedItem};
        fieldsChanged.Content.InnerItems = items;

        var original = new ProvisionVersion(fieldsOriginal);
        var changed = new ProvisionVersion(fieldsChanged);

        var difference = _differenceCalculator.CalculateDifferences(original, changed);
        
        Assert.Multiple(() =>
        {
            Assert.That(difference.AddedContent, Has.Count.EqualTo(1));
            Assert.That(difference.AddedContent.First(), Is.EqualTo(addedItem.Id));
            Assert.That(difference.RemovedContent, Has.Count.EqualTo(1));
            Assert.That(difference.RemovedContent.First(), Is.EqualTo(deletedItem.Id));
        });
    }
}