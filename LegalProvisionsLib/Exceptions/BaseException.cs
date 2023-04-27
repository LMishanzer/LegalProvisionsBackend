using System.Net;

namespace LegalProvisionsLib.Exceptions;

public abstract class BaseException : Exception
{
    public abstract HttpStatusCode HttpStatusCode { get; }

    protected BaseException(string? message = null, Exception? innerException = null) 
        : base(message, innerException)
    {
        
    }
}