using LegalProvisionsLib.Exceptions;

namespace LegalProvisionsLib.DataPersistence.Exceptions;

public class ElementsCountException : ClientSideException
{
    public ElementsCountException(string message) 
        : base(message)
    {
        
    }
}