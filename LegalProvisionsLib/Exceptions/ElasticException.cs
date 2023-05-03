namespace LegalProvisionsLib.Exceptions;

internal class ElasticException : Exception
{
    public int StatusCode { get; set; }

    public ElasticException(string message, Exception innerException) : base(message, innerException) { }

    public ElasticException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}