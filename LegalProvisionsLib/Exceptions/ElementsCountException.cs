namespace LegalProvisionsLib.Exceptions;

public class ElementsCountException : ClientSideException
{
    public ElementsCountException(string message) 
        : base(message)
    {
        
    }
}