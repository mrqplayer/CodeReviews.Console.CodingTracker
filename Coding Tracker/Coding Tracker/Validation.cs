using System.Globalization;

namespace Coding_Tracker;

public class Validation
{
    private const string Format = "dd-MM-yyyy HH:mm";

    public static bool IsValidDate(string date)
    {
        return DateTime.TryParseExact(date, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    public static bool IsEndDateValid(string startDate, string endDate)
    {
        DateTime start = DateTime.ParseExact(startDate, Format, CultureInfo.InvariantCulture);
        DateTime end = DateTime.ParseExact(endDate, Format, CultureInfo.InvariantCulture);

        return end > start;
    }
}