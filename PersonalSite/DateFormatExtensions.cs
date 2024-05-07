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
            return $"{((endDate == null ? now : endDate.GetValueOrDefault()) - startDate).Humanize(maxUnit: Humanizer.Localisation.TimeUnit.Year, minUnit: Humanizer.Localisation.TimeUnit.Month, precision: 2)}";
        }
    }
}