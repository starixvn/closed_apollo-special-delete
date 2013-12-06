// --------------------------------------------------------------------------
// <copyright file="SessionManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.11.22">Created</change>
// </history>

namespace Fanex.TicketControl.Core.Web
{
    using System;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// Get Value from Session with specific Type.
        /// </summary>
        /// <typeparam name="T">T parameter.</typeparam>
        /// <param name="sessionKey">Session Key.</param>
        /// <returns>Return T result.</returns>
        public static T Get<T>(string sessionKey)
        {
            try
            {
                T value = (T)HttpContext.Current.Session[sessionKey];
                return value;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Get value object from Session.
        /// </summary>
        /// <param name="sessionKey">Session Key.</param>
        /// <returns>Return object.</returns>
        public static object Get(string sessionKey)
        {
            return SessionManager.Get<object>(sessionKey);
        }

        public static void Set<T>(string sessionKey, T value)
        {
            try
            {
                HttpContext.Current.Session[sessionKey] = value;
            }
            catch (Exception)
            {
            }
        }

        public static void Set(string sessionKey, object value)
        {
            SessionManager.Set<object>(sessionKey, value);
        }
    }
}