// --------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header</change>
// </history>
namespace Fanex.TicketControl.Misc
{
    using System.Web.Mvc;

    /// <summary>
    /// Route config ticket listing.
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "Misc/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.TicketControl.Misc.Controllers" });
        }
    }
}