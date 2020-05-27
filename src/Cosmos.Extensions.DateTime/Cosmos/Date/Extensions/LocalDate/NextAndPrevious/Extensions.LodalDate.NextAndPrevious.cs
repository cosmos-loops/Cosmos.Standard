using System;
using NodaTime;

// ReSharper disable once CheckNamespace
namespace Cosmos.Date
{
    /// <summary>
    /// LocalDate extensions
    /// </summary>
    public static partial class LocalDateExtensions
    {
        /// <summary>
        /// Next year
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static LocalDate NextYear(this LocalDate ld)
        {
            var year = ld.Year + 1;
            var daysOfMonth = DateTime.DaysInMonth(year, ld.Month);

            if (daysOfMonth == ld.Day)
                return new LocalDate(ld.Era, year, ld.Month, ld.Day);

            var d = ld.Day - daysOfMonth;
            var p = new LocalDate(ld.Era, year, ld.Month, daysOfMonth);
            return p.AddDays(d);
        }

        /// <summary>
        /// Previous Year
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static LocalDate PreviousYear(this LocalDate ld)
        {
            var year = ld.Year - 1;
            var daysOfMonth = DateTime.DaysInMonth(year, ld.Month);

            if (daysOfMonth == ld.Day)
                return new LocalDate(ld.Era, year, ld.Month, ld.Day);

            var d = ld.Day - daysOfMonth;
            var p = new LocalDate(ld.Era, year, ld.Month, daysOfMonth);
            return p.AddDays(d);
        }

        /// <summary>
        /// Gets next month
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static LocalDate NextMonth(this LocalDate ld)
        {
            var year = ld.Month == 12 ? ld.Year + 1 : ld.Year;

            var month = ld.Month == 12 ? 1 : ld.Month + 1;

            var firstDayOfNextMonth = ld.SetDate(year, month, 1);

            var lastDayOfPreviousMonth = firstDayOfNextMonth.LastDayOfMonth().Day;

            var day = ld.Day > lastDayOfPreviousMonth ? lastDayOfPreviousMonth : ld.Day;

            return firstDayOfNextMonth.SetDay(day);
        }

        /// <summary>
        /// Gets previous month
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static LocalDate PreviousMonth(this LocalDate ld)
        {
            var year = ld.Month == 1 ? ld.Year - 1 : ld.Year;

            var month = ld.Month == 1 ? 12 : ld.Month - 1;

            var firstDayOfPreviousMonth = ld.SetDate(year, month, 1);

            var lastDayOfPreviousMonth = firstDayOfPreviousMonth.LastDayOfMonth().Day;

            var day = ld.Day > lastDayOfPreviousMonth ? lastDayOfPreviousMonth : ld.Day;

            return firstDayOfPreviousMonth.SetDay(day);
        }

        /// <summary>
        /// Next day
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static LocalDate NextDay(this LocalDate ld) => ld.AddDays(1);

        /// <summary>
        /// Previous Day
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static LocalDate PreviousDay(this LocalDate ld) => ld.AddDays(-1);

        /// <summary>
        /// Returns the next date with the specified day-of-week
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static LocalDate Next(this LocalDate ld, DayOfWeek dayOfWeek)
        {
            var dow = NodaTime.Helpers.DayOfWeekHelper.ToNodaTimeWeek(dayOfWeek);
            return DateAdjusters.Next(dow)(ld);
        }

        /// <summary>
        /// Returns the next date with the specified day-of-week
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static LocalDate Next(this LocalDate ld, IsoDayOfWeek dayOfWeek)
        {
            return DateAdjusters.Next(dayOfWeek)(ld);
        }

        /// <summary>
        /// Returns the next date with the specified day-of-week, or the original date, if the day is already correct.
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static LocalDate NextOrSame(this LocalDate ld, DayOfWeek dayOfWeek)
        {
            var dow = NodaTime.Helpers.DayOfWeekHelper.ToNodaTimeWeek(dayOfWeek);
            return DateAdjusters.NextOrSame(dow)(ld);
        }

        /// <summary>
        /// Returns the next date with the specified day-of-week, or the original date, if the day is already correct.
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static LocalDate NextOrSame(this LocalDate ld, IsoDayOfWeek dayOfWeek)
        {
            return DateAdjusters.NextOrSame(dayOfWeek)(ld);
        }

        /// <summary>
        /// Returns the previous date with the specified day-of-week
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static LocalDate Previous(this LocalDate ld, DayOfWeek dayOfWeek)
        {
            var dow = NodaTime.Helpers.DayOfWeekHelper.ToNodaTimeWeek(dayOfWeek);
            return DateAdjusters.Previous(dow)(ld);
        }

        /// <summary>
        /// Returns the previous date with the specified day-of-week
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static LocalDate Previous(this LocalDate ld, IsoDayOfWeek dayOfWeek)
        {
            return DateAdjusters.Previous(dayOfWeek)(ld);
        }

        /// <summary>
        /// Returns the previous date with the specified day-of-week, or the original date, if the day is already correct.
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static LocalDate PreviousOrSame(this LocalDate ld, DayOfWeek dayOfWeek)
        {
            var dow = NodaTime.Helpers.DayOfWeekHelper.ToNodaTimeWeek(dayOfWeek);
            return DateAdjusters.PreviousOrSame(dow)(ld);
        }

        /// <summary>
        /// Returns the previous date with the specified day-of-week, or the original date, if the day is already correct.
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static LocalDate PreviousOrSame(this LocalDate ld, IsoDayOfWeek dayOfWeek)
        {
            return DateAdjusters.PreviousOrSame(dayOfWeek)(ld);
        }

        /// <summary>
        /// Week after
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static LocalDate WeekAfter(this LocalDate ld) => ld.AddWeeks(1);

        /// <summary>
        /// Week before
        /// </summary>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static LocalDate WeekBefore(this LocalDate ld) => ld.AddWeeks(-1);
    }
}