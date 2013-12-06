// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header</change>
// </history>

namespace Fanex.TicketControl.Website.Areas.SportBook.Area_Start
{
    using System.Web.Optimization;

    /// <summary>
    ///     Bundle Config.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Register bundle for Sport-book.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/SportBook/VoidSingleBet").Include(
                "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js",
                "~/Areas/SportBook/Scripts/VoidSingleBet/VoidSingleBet.js",
                "~/Areas/SportBook/Scripts/VoidSingleBet/VoidSingleBetHelper.js",
                "~/Areas/SportBook/Scripts/VoidSingleBet/VoidSingleBetValidateData.js",
                "~/Areas/SportBook/Scripts/VoidSingleBet/VoidSingleBetManager.js"));

            bundles.Add(new ScriptBundle("~/areas/SportBook/VoidMixParlayBet").Include(
                "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js",
                "~/Areas/SportBook/Scripts/VoidMixParlayBet/VoidMixParlayBetHelper.js",
                "~/Areas/SportBook/Scripts/VoidMixParlayBet/VoidMixParlayBetValidateData.js",
                "~/Areas/SportBook/Scripts/VoidMixParlayBet/VoidMixParlayBet.js",
                "~/Areas/SportBook/Scripts/VoidMixParlayBet/SubTicketMixParlay.js",
                "~/Areas/SportBook/Scripts/VoidMixParlayBet/VoidMixParlayBetManager.js"));

            bundles.Add(new ScriptBundle("~/areas/SportBook/RecoverBet").Include(
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js",
                "~/Areas/SportBook/Scripts/RecoverBet/RecoverBet.js"));

            bundles.Add(new ScriptBundle("~/areas/SportBook/VoidOutrightBet").Include(
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js",
                "~/Areas/SportBook/Scripts/VoidOutrightBet/VoidOutrightBet.js",
                "~/Areas/SportBook/Scripts/VoidOutrightBet/VoidOutrightBetValidateData.js"));

            bundles.Add(new ScriptBundle("~/areas/SportBook/VoidEarlyKOBet").Include(
               "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
               "~/Scripts/_Libs/Utils/stdlib.js",
               "~/Scripts/Common/Message.Helper.js",
               "~/Scripts/Common/Message.Validator.js",
               "~/Areas/SportBook/Scripts/VoidEarlyKOBet/VoidEarlyKOBetHelper.js",
               "~/Areas/SportBook/Scripts/VoidEarlyKOBet/VoidEarlyKOBetValidateData.js",
               "~/Areas/SportBook/Scripts/VoidEarlyKOBet/VoidEarlyKOBet.js",
               "~/Areas/SportBook/Scripts/VoidEarlyKOBet/SubTicketMixParlay.js",
               "~/Areas/SportBook/Scripts/VoidEarlyKOBet/VoidEarlyKOBetManager.js"));
            VoidByBetId(bundles);
        }

        /// <summary>
        /// Bundle for Void bet Id.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        /// <returns>Return collection.</returns>
        private static BundleCollection VoidByBetId(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/SportBook/VoidByBetId").Include(
         "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
         "~/Scripts/_Libs/Utils/stdlib.js",
         "~/Scripts/Common/Message.Helper.js",
         "~/Scripts/Common/Message.Validator.js",
          "~/Areas/SportBook/Scripts/VoidByBetId/VoidByBetIdValidateData.js",
         "~/Areas/SportBook/Scripts/VoidByBetId/VoidByBetId.js",
         "~/Areas/SportBook/Scripts/VoidByBetId/VoidByBetIdManager.js"));
            return bundles;
        }
    }
}