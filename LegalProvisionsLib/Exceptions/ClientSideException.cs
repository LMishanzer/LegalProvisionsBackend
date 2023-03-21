namespace LegalProvisionsLib.Exceptions;

public class ClientSideException : Exception
{
    public ClientSideException(string? message = null, Exception? innerException = null) 
        : base(message, innerException)
    {
        
    }
}