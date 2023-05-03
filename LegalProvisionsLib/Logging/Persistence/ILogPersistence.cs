using LegalProvisionsLib.Logging.Models;

namespace LegalProvisionsLib.Logging.Persistence;

public interface ILogPersistence
{
    Task SaveLogAsync(LogItem item);
}