// --------------------------------------------------------------------------
// <copyright file="UserControlAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.24">Created</change>
// </history>

namespace Fanex.TicketControl.Website.Areas.UserControl
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using Fanex.TicketControl.Website.App_Start;

    /// <summary>
    /// Area registration user.
    /// </summary>
    public class UserControlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "UserControl"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            context.MapRoute(
                AreaName + "_Default",
                "UserControl/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.TicketControl.Website.Areas.UserControl.Controllers" });
        }
    }
}