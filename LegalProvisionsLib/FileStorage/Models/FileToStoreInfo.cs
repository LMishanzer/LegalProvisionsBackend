namespace LegalProvisionsLib.FileStorage.Models;

public class FileToStoreInfo
{
    public FileToStoreInfo() { }
    
    public FileToStoreInfo(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = string.Empty;
}