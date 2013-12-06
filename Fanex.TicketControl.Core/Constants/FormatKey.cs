// ----------------------------------------------------------------------
// <copyright file="FormatKey.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Constants
{
    /// <summary>
    /// Formatter for date and number.
    /// </summary>
    public class FormatKey
    {
        public const string LongDateFormat = "dd MMM, yyyy hh:mm:ss tt";
        public const string LongDateFormatWithSlash = "MM/dd/yyyy hh:mm:ss tt";
        public const string MonthYearDateFormat = "MMM yyyy";
        public const string MonthYearDateFormatWithSlash = "MM/yyyy";
        public const string ShortDateFormat = "MMM dd, yyyy";
        public const string ShortDateFormatWithSlash = "MM/dd/yyyy";
    }
}