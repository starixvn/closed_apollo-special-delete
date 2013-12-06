// --------------------------------------------------------------------------
// <copyright file="UserLoginAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ethan.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.TicketControl.Website.Areas.UserLogin
{
    using System.Web.Mvc;

    /// <summary>
    /// Area registration user.
    /// </summary>
    public class UserLoginAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserLogin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserLogin_default",
                "UserLogin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}