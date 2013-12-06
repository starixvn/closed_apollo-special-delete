// --------------------------------------------------------------------------
// <copyright file="MiscAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header</change>
// </history>
namespace Fanex.TicketControl.Misc
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Area Registration ticket listing.
    /// </summary>
    public class MiscAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Misc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}