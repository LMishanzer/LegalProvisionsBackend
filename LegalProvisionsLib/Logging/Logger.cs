using LegalProvisionsLib.Logging.Models;
using LegalProvisionsLib.Logging.Persistence;

namespace LegalProvisionsLib.Logging;

public class Logger : ILogger
{
    private const string ExceptionsFile = "exceptions.txt";
    
    private readonly ILogPersistence _logPersistence;

    public Logger(ILogPersistence logPersistence)
    {
        _logPersistence = logPersistence;
    }
    
    public async void LogException(Exception exception)
    {
        try
        {
            await _logPersistence.SaveLogAsync(new ErrorLog
            {
                Exception = exception
            });
        }
        catch (Exception e)
        {
            await File.WriteAllTextAsync(ExceptionsFile, e.ToString());
        }
    }

    public async void LogRequest(RequestLog requestLog)
    {
        await _logPersistence.SaveLogAsync(requestLog);
    }
}