using LegalProvisionsLib.Logging.Models;
using LegalProvisionsLib.Logging.Persistence;
using LegalProvisionsLib.Settings;

namespace LegalProvisionsLib.Logging;

public class Logger : ILogger
{
    private static readonly string ExceptionsFile = Path.Combine(StaticSettings.PersistantFileStorage, "exceptions.txt");
    private static readonly Mutex ExceptionsFileMutex = new();
    
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
            await FallbackLogAsync(e, exception);
        }
    }

    public async void LogRequest(RequestLog requestLog)
    {
        try
        {
            await _logPersistence.SaveLogAsync(requestLog);
        }
        catch (Exception e)
        {
            await FallbackLogAsync(e);
        }
    }

    private static async Task FallbackLogAsync(params Exception[] exceptions)
    {
        try
        {
            foreach (var exception in exceptions)
            {
                await LogExceptionToFileAsync(exception);
            }
        }
        catch (Exception)
        {
            // ignore
        }
    }

    private static async Task LogExceptionToFileAsync(Exception exception)
    {
        if (!ExceptionsFileMutex.WaitOne(TimeSpan.FromSeconds(10)))
        {
            return;
        }

        try
        {
            await File.AppendAllTextAsync(ExceptionsFile, exception.ToString());
        }
        finally
        {
            ExceptionsFileMutex.ReleaseMutex();
        }
    }
}