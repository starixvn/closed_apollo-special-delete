// ----------------------------------------------------------------------
// <copyright file="LoggingHandler.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Services.Impl
{
    using System;

    using Fanex.Logging;

    public enum LogType : int
    {
        /// <summary>
        /// Information log.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Error log.
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoggingHandler
    {
        public static void Log(Exception ex)
        {
            LogService.Error(ex.Message, ex);
        }

        /// <summary>
        /// Write log info.
        /// </summary>
        /// <param name="message">Message value.</param>
        /// <param name="logType">Log type value.</param>
        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            switch (logType)
            {
                case LogType.Info:
                    LogService.Info(message);
                    break;
                case LogType.Error:
                    LogService.Error(message);
                    break;
            }
        }
    }
}