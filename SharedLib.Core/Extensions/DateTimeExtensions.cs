namespace SharedLib.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? SetKindUtc(this DateTime? dateTime)
    {
        return dateTime?.SetKindUtc().AddHours(7);
    }

    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        //return dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        return dateTime == DateTime.UtcNow.AddHours(7) ? dateTime : DateTime.UtcNow.AddHours(7);
    }
}