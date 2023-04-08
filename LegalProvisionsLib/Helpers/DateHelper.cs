namespace LegalProvisionsLib.Helpers;

public static class DateHelper
{
    public static DateOnly DateTimeToDate(DateTime dateTime)
    {
        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}