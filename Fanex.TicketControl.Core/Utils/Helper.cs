// ----------------------------------------------------------------------
// <copyright file="Helper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Helper
    {
        public static DateTime ConvertDatetimeZone(DateTime datetime, string distinationTimeZoneId)
        {
            var utcTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(datetime, distinationTimeZoneId);
            return utcTime;
        }

        /// <summary>
        /// Convert Date time Zone GMT-4 (Canada).
        /// </summary>
        /// <param name="datetime">Date Time.</param>
        /// <returns>Return date time.</returns>
        public static DateTime ConvertDatetimeZoneCanada(DateTime datetime)
        {
            return datetime.AddHours(-12);
        }

        /// <summary>
        /// Get IP on client.
        /// </summary>
        /// <returns>Return IP.</returns>
        public static string GetClientIp()
        {
            var ip = string.Empty;
            try
            {
                ip = HttpContext.Current.Request.IsLocal ? "127.0.0.1" : Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return ip;
        }
    }
}