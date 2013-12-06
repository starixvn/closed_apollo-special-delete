// --------------------------------------------------------------------------
// <copyright file="BetTeamText.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.05">Create</change>
// </history>

namespace Fanex.TicketControl.Core.Utils
{
    using Fanex.TicketControl.Core.Constants;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class BetTeamText
    {
        /// <summary>
        /// Simulation of f_GetChoice.
        /// </summary>
        /// <param name="betteam">Bet team value.</param>
        /// <param name="bettype">Bet type value.</param>
        /// <returns>The actual bet team name.</returns>
        public static string GetBetTeam(string betteam, int bettype)
        {
            if (string.IsNullOrEmpty(betteam) || betteam.Length < 2)
            {
                return betteam;
            }

            if (bettype == BetTypeConstant.Fglg)
            {
                var ha = betteam.Substring(0, 1);
                var fglg = betteam.Substring(betteam.Length - 1, 1);

                return GetHa(ha) + GetFglg(fglg);
            }

            if (bettype == BetTypeConstant.Htft)
            {
                var home = betteam.Substring(0, 1);
                var away = betteam.Substring(betteam.Length - 1, 1);

                return GetHome(home) + GetHome(away);
            }

            return betteam;
        }

        /// <summary>
        /// Get Hone and away.
        /// </summary>
        /// <param name="ha">Home or away.</param>
        /// <returns>Return value.</returns>
        private static string GetHa(string ha)
        {
            switch (ha)
            {
                case "1":
                    return "Home";
                case "2":
                    return "Away";
                default:
                    return "No";
            }
        }

        /// <summary>
        /// First goal or last goal.
        /// </summary>
        /// <param name="fglg">First goal, last goal.</param>
        /// <returns>Return value.</returns>
        private static string GetFglg(string fglg)
        {
            switch (fglg)
            {
                case "1":
                    return " FG";
                case "2":
                    return " LG";
                default:
                    return "Goal";
            }
        }

        /// <summary>
        /// Get home, away.
        /// </summary>
        /// <param name="home">Home or away.</param>
        /// <returns>Return value.</returns>
        private static string GetHome(string home)
        {
            switch (home)
            {
                case "1":
                    return "H";
                case "2":
                    return "A";
                default:
                    return "D";
            }
        }
    }
}