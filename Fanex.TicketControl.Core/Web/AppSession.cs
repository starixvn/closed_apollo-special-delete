// --------------------------------------------------------------------------
// <copyright file="AppSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Bao" date="2012.10.18">Created</change>
//   <change who="Ricky.Nguyen" date="2012.11.22">Update Session Manager</change>
// </history>

namespace Fanex.TicketControl.Core.Web
{
    using System;
    using System.Collections.Generic;

    using Fanex.NAP.Core.Model;
    using Fanex.TicketControl.Core.Constants;

    /// <summary>
    /// AppSession class.
    /// </summary>
    public static class AppSession
    {
        /// <summary>
        /// Gets or sets IsTrackedMenu: determine current working on menu is.
        /// tracked for page tracking or not.
        /// </summary>
        /// <value>Current Action Name.</value>
        public static string CurrentActionName
        {
            get
            {
                return SessionManager.Get<string>(SessionKey.CurrentActionName);
            }

            set
            {
                SessionManager.Set<string>(SessionKey.CurrentActionName, value);
            }
        }

        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>Current User.</value>
        public static User CurrentUser
        {
            get
            {
                return SessionManager.Get<User>(SessionKey.CurrentLogInUser);
            }

            set
            {
                SessionManager.Set<User>(SessionKey.CurrentLogInUser, value);
            }
        }

        /// <summary>
        /// Gets or sets menu of current login user.
        /// </summary>
        /// <value>Current User Menu.</value>
        public static IList<MenuItem> CurrentUserMenu
        {
            get
            {
                return SessionManager.Get<IList<MenuItem>>(SessionKey.CurrentUserMenu);
            }

            set
            {
                SessionManager.Set<IList<MenuItem>>(SessionKey.CurrentUserMenu, value);
            }
        }

        /// <summary>
        /// Gets or sets current working menu url.
        /// </summary>
        /// <value>Current Working On Menu.</value>
        public static MenuItem CurrentWorkingOnMenu
        {
            get
            {
                return SessionManager.Get<MenuItem>(SessionKey.CurrentWorkingOnMenu);
            }

            set
            {
                SessionManager.Set<MenuItem>(SessionKey.CurrentWorkingOnMenu, value);
            }
        }

        public static Dictionary<string, int> RoleAccess
        {
            get
            {
                try
                {
                    return SessionManager.Get<Dictionary<string, int>>(SessionKey.CurrentUserRoleAccess);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            set
            {
                SessionManager.Set<Dictionary<string, int>>(SessionKey.CurrentUserRoleAccess, value);
            }
        }

        public static bool ViewIpvn
        {
            get
            {
                return SessionManager.Get<bool>(SessionKey.ViewIpVn);
            }

            set
            {
                SessionManager.Set<bool>(SessionKey.ViewIpVn, value);
            }
        }
    }
}