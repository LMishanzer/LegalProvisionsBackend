namespace LegalProvisionsLib.DataPersistence.Models;

public class TextElement
{
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }

    public TextElementType ElementType { get; set; }
    public string? Comment { get; set; }
    public Reference? Reference { get; set; }
}

public enum TextElementType
{
    Reference,
    Comment
}