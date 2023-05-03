namespace LegalProvisionsLib.Logging.Models;

public class ErrorLog : LogItem
{
    public required Exception Exception { get; init; }
}