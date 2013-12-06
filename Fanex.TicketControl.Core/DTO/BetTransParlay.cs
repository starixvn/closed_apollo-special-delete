// --------------------------------------------------------------------------
// <copyright file="BetTransParlay.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.23">Create</change>
//   <change who="Kaden.Trinh" date="2013.10.23">Refactor code. Move from sportsbook library.</change>
// </history>

namespace Fanex.TicketControl.Core.DTO
{
    /// <summary>
    /// DTO Bet trans parlay.
    /// </summary>
    public class BetTransParlay : BetTrans
    {
        public long SubTransId { get; set; }

        public short Choice_LiveHomeScore { get; set; }

        public short Choice_LiveAwayScore { get; set; }

        public decimal Choice_Odds { get; set; }

        public string Choice_BetTeam { get; set; }

        public int Choice_BetType { get; set; }

        public long Choice_HomeId { get; set; }

        public long Choice_AwayId { get; set; }

        public bool Choice_IsLiveIndicator { get; set; }

        public decimal MOdds { get; set; }

        public decimal Choice_Hdp1 { get; set; }

        public decimal Choice_Hdp2 { get; set; }

        public int Choice_LeagueId { get; set; }

        public int Choice_SportType { get; set; }

        public string RefNo { get; set; }
    }
}