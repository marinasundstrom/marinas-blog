using System;
using System.Collections.Generic;

using System.Globalization;

using Humanizer;
using Humanizer.Localisation;

using PersonalSite.Models;

namespace PersonalSite
{
    public static class DateFormatExtensions
    {
        public static string Humanize(this DateTime startDate, DateTime? endDate)
        {
            return $"{startDate.ToString("MMM yyyy", CultureInfo.InvariantCulture)} - {(endDate == null ? "Present" : endDate.GetValueOrDefault().ToString("MMM yyyy", CultureInfo.InvariantCulture))}";
        }

        public static string Humanize2(this DateTime startDate, DateTime? endDate)
        {
            var now = ExperienceExtensions.GetNowDate();
            var effectiveEnd = endDate ?? now;

            int monthDiff = ((effectiveEnd.Year - startDate.Year) * 12) + effectiveEnd.Month - startDate.Month;

            // If there are leftover days, round up the month count
            if (effectiveEnd.Day > startDate.Day ||
                (effectiveEnd.Day == startDate.Day && (effectiveEnd.TimeOfDay > startDate.TimeOfDay)))
            {
                monthDiff += 1;
            }

            // Edge case: if dates are the same day or before, treat as 1 month minimum
            if (monthDiff <= 0)
            {
                monthDiff = 1;
            }

            if (monthDiff < 12)
            {
                return $"{monthDiff} month{(monthDiff == 1 ? "" : "s")}";
            }
            else
            {
                int years = monthDiff / 12;
                int remainingMonths = monthDiff % 12;
                return remainingMonths > 0
                    ? $"{years} year{(years == 1 ? "" : "s")} {remainingMonths} month{(remainingMonths == 1 ? "" : "s")}"
                    : $"{years} year{(years == 1 ? "" : "s")}";
            }
        }
    }
}