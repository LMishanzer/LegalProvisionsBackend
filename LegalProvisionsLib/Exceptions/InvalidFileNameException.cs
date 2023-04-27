using System.Net;

namespace LegalProvisionsLib.Exceptions;

public class InvalidFileNameException : BaseException
{
    public override HttpStatusCode HttpStatusCode => HttpStatusCode.BadRequest;
    
    public InvalidFileNameException(string? message) : base(message ?? "File name cannot be empty.")
    {
        
    }
}