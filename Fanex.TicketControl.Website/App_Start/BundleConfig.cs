// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.28">Support bundle mode release</change>
// </history>
namespace Fanex.TicketControl.Website.App_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.ReadabilityRules", "SA1116:SplitParametersMustStartOnLineAfterDeclaration",
            Justification = "Reviewed. Suppression is OK here.")]
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            CSSBundles(bundles);
            JSBundles(bundles);
        }

        /// <summary>
        /// CSS Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void CSSBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude("~/css/libs",
           "~/content/jquery-ui/jquery-ui-1.9.1.custom.min.css",
           "~/scripts/_libs/fanexgrid/fanex.grid.css",
           "~/scripts/_libs/fanexgrid/fanex.pagination.css",
           "~/scripts/_Libs/Validation/validationEngine.jquery.css",
           "~/Content/BetList.css",
           "~/Scripts/_Libs/FanexGrid/Fanex.Pagination.css");
        }

        /// <summary>
        /// JQuery bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void JSBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/libs").Include(
                "~/Scripts/_Libs/jQuery/jquery-1.8.2.js",
                "~/Scripts/_Libs/jQuery/jquery-ui-1.9.1.js",
                "~/Scripts/_Libs/jQuery/jquery.formattyping.js",
                "~/Scripts/_Libs/Bootstrap/bootstrap-alert.js",
                "~/Scripts/_Libs/Bootstrap/bootstrap-tooltip.js",
                "~/Scripts/_Libs/BlockUI/jquery.blockUI.1.33.js",
                "~/Scripts/_Libs/FanexGrid/Fanex.Grid.js",
                "~/Scripts/_Libs/FanexGrid/Fanex.Pagination.js",
                "~/Scripts/_Libs/FormatLibs/NumberFormat/numeral.js",
                "~/Scripts/NapTicketControl.js",
                "~/Scripts/NapTicketControl.Helper.js",
                "~/Scripts/_Libs/Validation/jquery.validationEngine-en.js",
                "~/Scripts/_Libs/Validation/jquery.validationEngine.js",
                "~/Scripts/_Libs/Utils/date.format.js"));
        }
    }

    /// <summary>
    /// Bundle Extensions.
    /// </summary>
    [SuppressMessage(
             "Microsoft.StyleCop.CSharp.MaintainabilityRules",
             "SA1402:FileMayOnlyContainASingleClass",
             Justification = "Splitting this file into classes would get too confusing.")]
    internal static class BundleExtensions
    {
        /// <summary>
        /// Add And Include.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        /// <param name="bundleVirtualPath">BundleV Virtual Path.</param>
        /// <param name="virtualPaths">Virtual Path.</param>
        public static void AddAndInclude(
            this BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new CssTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }

        /// <summary>
        /// Relative From Absolute Path.
        /// </summary>
        /// <param name="context">Context value.</param>
        /// <param name="path">Path value.</param>
        /// <returns>Return string.</returns>
        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
}