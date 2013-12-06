// --------------------------------------------------------------------------
// <copyright file="SportBookAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header</change>
// </history>

namespace Fanex.TicketControl.Website.Areas.SportBook
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using Fanex.TicketControl.SportBook;
    using Fanex.TicketControl.Website.Areas.SportBook.Area_Start;

    /// <summary>
    /// Sport Book Area Registration.
    /// </summary>
    public class SportBookAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SportBook";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}