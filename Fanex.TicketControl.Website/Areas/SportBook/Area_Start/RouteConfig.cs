// --------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Create</change>
// </history>
namespace Fanex.TicketControl.SportBook
{
    using System.Web.Mvc;

    /// <summary>
    /// Route config for sport-book.
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "SportBook/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.TicketControl.SportBook.Controllers" });
        }
    }
}