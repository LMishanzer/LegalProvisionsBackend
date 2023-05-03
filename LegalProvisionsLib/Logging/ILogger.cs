using LegalProvisionsLib.Logging.Models;

namespace LegalProvisionsLib.Logging;

public interface ILogger
{
    void LogException(Exception exception);
    void LogRequest(RequestLog requestLog);
}