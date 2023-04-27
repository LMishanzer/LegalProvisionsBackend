namespace LegalProvisionsLib.FileStorage.Models;

public class FileToStore
{
    public string Name { get; set; }
    public Stream Content { get; set; }
    
    public FileToStore(string name, Stream content)
    {
        Name = name;
        Content = content;
    }
}