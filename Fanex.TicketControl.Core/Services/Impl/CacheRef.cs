// ----------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Services.Impl
{
    using Fanex.Common;

    using RemotingRef;

    /// <summary>
    /// Cache ref.
    /// </summary>
    public static class CacheRef
    {
        public static string GetBettype(object bettypeId)
        {
            return GetBettype(bettypeId, null);
        }

        public static string GetBettype(object bettypeId, object betID)
        {
            // resourceId remove first character
            string resourceId = string.Empty;
            if (betID != null && betID.ToString().Length > 1)
            {
                resourceId = betID.ToString().Substring(1);
            }

            return RefData.GetBetTypeName(bettypeId.ToString(), TWLang("en-US"), string.Empty, resourceId);
        }

        public static string GetBettypeNameSportingSolution(object betypeId, string resourceId)
        {
            string bettypeName = string.Empty;
            bettypeName = RefData.GetBetTypeName(Converter.ToString2(betypeId), TWLang("en-US"), string.Empty, resourceId);
            return bettypeName;
        }

        public static string GetCasinoBetItem(object betStatusId)
        {
            return RefData.GetCasinoBetItem(betStatusId.ToString(), TWLang("en-US"), string.Empty);
        }

        /// <summary>
        /// Get combination name.
        /// </summary>
        /// <param name="betteam">Bet team name.</param>
        /// <returns>Return string.</returns>
        public static string GetCombinationName(string betteam)
        {
            var name = string.Empty;
            switch (betteam)
            {
                case "110":
                    name = "Trixie";
                    break;
                case "1110":
                    name = "Yankee";
                    break;
                case "11110":
                    name = "Canadian";
                    break;
                case "111110":
                    name = "Heinz";
                    break;
                case "1111110":
                    name = "Super Heinz";
                    break;
                case "11111110":
                    name = "Goliath";
                    break;
            }

            return name;
        }

        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(Converter.ToString2(indexId), TWLang("en-US"));
        }

        public static string GetHorseTeamName(object codeID)
        {
            return RefData.GetHorseTeamName(Converter.ToString2(codeID), TWLang("en-US"));
        }

        public static string GetLeagueName(object leagueID)
        {
            return RefData.GetLeagueName(Converter.ToString2(leagueID), TWLang("en-US"), string.Empty);
        }

        public static string GetLiveCasinoResult(int resultId)
        {
            string result = string.Empty;

            return RefData.GetCasinoBetItem(Converter.ToString2(resultId), TWLang("en-US"));
        }

        public static string GetOddsTypeName(object oddsType)
        {
            return RefData.GetOddsTypeName(Converter.ToString2(oddsType), TWLang("en-US"));
        }

        public static string GetRouletteBetItem(int itemId)
        {
            return RefData.GetRouletteBetItem(Converter.ToString2(itemId), TWLang("en-US"));
        }

        public static string GetRouletteBetItem(object itemId)
        {
            return RefData.GetRouletteBetItem(itemId.ToString(), TWLang("en-US"), string.Empty);
        }

        public static string GetSportName(object sportID)
        {
            return RefData.GetSportName(Converter.ToString2(sportID), TWLang("en-US"), string.Empty);
        }

        public static string GetTeamName(object teamID)
        {
            return RefData.GetTeamName(Converter.ToString2(teamID), TWLang("en-US"), string.Empty);
        }

        /// <summary>
        /// Get lang value.
        /// </summary>
        /// <param name="lang">Lang key value.</param>
        /// <returns>Return string.</returns>
        private static string TWLang(string lang)
        {
            switch (lang)
            {
                case "en-US":
                    return "en";
                case "zh-TW":
                    return "ch"; // Traditional
                case "zh-CN":
                    return "cs"; // Simplified
                case "ja-JP":
                    return "jp";
                case "th-TH":
                    return "th"; // Thai
                default:
                    return "en";
            }
        }
    }
}