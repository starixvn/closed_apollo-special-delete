// ----------------------------------------------------------------------
// <copyright file="OddsText.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/10/07">Created</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Utils
{
    using System.Collections.Generic;
    using System.Xml;
    using Constants;
    using Services.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OddsText
    {
        private static readonly IList<int> BetTypeOfDecimalOdds;

        static OddsText()
        {
            BetTypeOfDecimalOdds = new List<int>() { 1, 2, 3, 7, 12, 8, 20, 124, 125 };

            for (var i = 81; i <= 88; i++)
            {
                BetTypeOfDecimalOdds.Add(i);
            }

            for (var i = 91; i <= 120; i++)
            {
                BetTypeOfDecimalOdds.Add(i);
            }
        }

        /// <summary>
        /// Simulation of f_GetOddsByBetType.
        /// </summary>
        /// <param name="odds">Odds value.</param>
        /// <param name="bettype">Bet type value.</param>
        /// <param name="oddsType">Odds type value.</param>
        /// <returns>Actual odds.</returns>
        public static decimal GetOdds(decimal odds, int bettype, int oddsType)
        {
            if (BetTypeOfDecimalOdds.Contains(bettype) && oddsType == OddsTypeConstant.Decimal)
            {
                return odds + 1;
            }

            return BetTypeOfDecimalOdds.Contains(bettype) && oddsType == OddsTypeConstant.Us ? odds * 100 : odds;
        }

        public static int GetOddsType(int bettype, int oddsType)
        {
            return BetTypeOfDecimalOdds.Contains(bettype) ? oddsType : 0;
        }

        public static string GetOddsTypeName(int betType, int oddsType)
        {
            oddsType = GetOddsType(betType, oddsType);
            return CacheRef.GetOddsTypeName(oddsType);
        }

        /// <summary>
        ///     Used to parse Odd html.
        ///     and get odd name.
        /// </summary>
        /// <param name="oddsHtml">Odds html.</param>
        /// <returns>Return odd name.</returns>
        public static string GetOddsName(string oddsHtml)
        {
            var valueNode = string.Empty;
            var xmlDoc = new XmlDocument();
            oddsHtml = "<div>" + oddsHtml + "</div>";
            xmlDoc.LoadXml(oddsHtml);
            var foundSpanNodes = xmlDoc.SelectNodes("//span");

            if (foundSpanNodes != null && foundSpanNodes.Count == 2)
            {
                valueNode = foundSpanNodes[1].InnerText.Trim();
            }

            return valueNode;
        }
    }
}