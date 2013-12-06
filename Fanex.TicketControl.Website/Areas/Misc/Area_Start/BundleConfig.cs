// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header bundle</change>
// </history>
namespace Fanex.TicketControl.Misc
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        ///     CSS Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/misc/ticketlisting").Include(
                "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Areas/Misc/Scripts/TicketListing.js",
                "~/Areas/Misc/Scripts/TicketListingManager.js"));

            bundles.Add(new ScriptBundle("~/areas/misc/BatchMessage").Include(
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessageHelper.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessageValidateData.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessage.js"));
        }
    }
}