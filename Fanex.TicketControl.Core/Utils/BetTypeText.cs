// --------------------------------------------------------------------------
// <copyright file="BetTypeText.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>

namespace Fanex.TicketControl.Core.Utils
{
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.Services.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class BetTypeText
    {
        private const string VirtualSport = @"Virtual Sports";

        public static string GetBetTypeName(int bettypeId, string betId)
        {
            var betypeName = bettypeId >= BetTypeConstant.MinSportingSolutionBetType && bettypeId <= BetTypeConstant.MaxSportingSolutionBetType
                ? CacheRef.GetBettypeNameSportingSolution(bettypeId, betId)
                : CacheRef.GetBettype(bettypeId);

            return BetTypeConstant.MinVirtualSportBetType <= bettypeId && bettypeId < BetTypeConstant.MaxVirtualSportBetType
                ? betypeName + VirtualSport
                : betypeName;
        }

        public static string GetBetTypeName(int bettypeId)
        {
            var betypeName = CacheRef.GetBettype(bettypeId);
            return BetTypeConstant.MinVirtualSportBetType <= bettypeId && bettypeId < BetTypeConstant.MaxVirtualSportBetType
                ? betypeName + VirtualSport
                : betypeName;
        }

        /// <summary>
        /// Get bet type name.
        /// </summary>
        /// <param name="bettype">Bet type value.</param>
        /// <param name="liveIndicator">Live Indicator value.</param>
        /// <returns>Return value.</returns>
        public static string GetBetTypeName(int bettype, bool liveIndicator)
        {
            var bettypeName = CacheRef.GetBettype(bettype);

            switch (bettype)
            {
                case 86:
                    bettypeName = liveIndicator ? "Next Odd/Even" : "FT. Odd/Even";
                    break;
                case 85:
                    bettypeName = "Next Over/Under";
                    break;
                case 90:
                    bettypeName += liveIndicator ? " (Live Game)" : " (Pre Game)";
                    break;
            }

            return bettypeName;
        }
    }
}