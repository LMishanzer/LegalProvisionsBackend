namespace LegalProvisionsLib.DataPersistence.Models;

public class Article
{
    public string Title { get; set; }

    public IEnumerable<string> Paragraphs { get; set; }
}