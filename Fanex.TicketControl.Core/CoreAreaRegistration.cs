// ----------------------------------------------------------------------
// <copyright file="CoreAreaRegistration.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    /// <summary>
    /// Override area.
    /// </summary>
    public class CoreAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Core";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Core_default",
                "Core/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.TicketControl.Core.Controllers" });
        }
    }
}