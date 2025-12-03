using MeetingManager.Domain.Enums;
using System.Globalization;

namespace MeetingManager.Mvc.Helpers;

/// <summary>
/// Helper class for formatting data using Arabic locale and standards
/// </summary>
public static class ArabicFormatHelper
{
    private static readonly CultureInfo ArabicCulture = new CultureInfo("ar-EG");

    /// <summary>
    /// Formats a DateTime to Arabic date format (dd/MM/yyyy)
    /// </summary>
    /// <param name="date">The date to format</param>
    /// <returns>Formatted date string in dd/MM/yyyy format</returns>
    public static string FormatDate(DateTime date)
    {
        return date.ToString("dd/MM/yyyy", ArabicCulture);
    }

    /// <summary>
    /// Formats a DateTime to Arabic date and time format (dd/MM/yyyy HH:mm)
    /// </summary>
    /// <param name="dateTime">The date time to format</param>
    /// <returns>Formatted date time string</returns>
    public static string FormatDateTime(DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy HH:mm", ArabicCulture);
    }

    /// <summary>
    /// Formats a number using Arabic-Egypt locale
    /// </summary>
    /// <param name="number">The number to format</param>
    /// <returns>Formatted number string</returns>
    public static string FormatNumber(int number)
    {
        return number.ToString("N0", ArabicCulture);
    }

    /// <summary>
    /// Formats a decimal number using Arabic-Egypt locale
    /// </summary>
    /// <param name="number">The decimal number to format</param>
    /// <param name="decimalPlaces">Number of decimal places</param>
    /// <returns>Formatted decimal number string</returns>
    public static string FormatDecimal(decimal number, int decimalPlaces = 2)
    {
        return number.ToString($"N{decimalPlaces}", ArabicCulture);
    }

    /// <summary>
    /// Gets Arabic text for MeetingRequestStatus enum
    /// </summary>
    /// <param name="status">The status enum value</param>
    /// <returns>Arabic status text</returns>
    public static string GetStatusText(MeetingRequestStatus status)
    {
        return status switch
        {
            MeetingRequestStatus.Pending => "قيد الانتظار",
            MeetingRequestStatus.Accepted => "مقبول",
            MeetingRequestStatus.Rejected => "مرفوض",
            _ => status.ToString()
        };
    }

    /// <summary>
    /// Gets Arabic text for UserRole enum
    /// </summary>
    /// <param name="role">The role enum value</param>
    /// <returns>Arabic role text</returns>
    public static string GetRoleText(UserRole role)
    {
        return role switch
        {
            UserRole.Secretary => "سكرتير",
            UserRole.Manager => "مدير",
            UserRole.Admin => "مسؤول",
            _ => role.ToString()
        };
    }

    /// <summary>
    /// Gets Bootstrap CSS class for status badge
    /// </summary>
    /// <param name="status">The status enum value</param>
    /// <returns>Bootstrap CSS class name</returns>
    public static string GetStatusBadgeClass(MeetingRequestStatus status)
    {
        return status switch
        {
            MeetingRequestStatus.Pending => "bg-warning text-dark",
            MeetingRequestStatus.Accepted => "bg-success",
            MeetingRequestStatus.Rejected => "bg-danger",
            _ => "bg-secondary"
        };
    }

    /// <summary>
    /// Gets Bootstrap icon for status
    /// </summary>
    /// <param name="status">The status enum value</param>
    /// <returns>Bootstrap icon CSS class</returns>
    public static string GetStatusIcon(MeetingRequestStatus status)
    {
        return status switch
        {
            MeetingRequestStatus.Pending => "bi-clock-history",
            MeetingRequestStatus.Accepted => "bi-check-circle-fill",
            MeetingRequestStatus.Rejected => "bi-x-circle-fill",
            _ => "bi-question-circle"
        };
    }
}
