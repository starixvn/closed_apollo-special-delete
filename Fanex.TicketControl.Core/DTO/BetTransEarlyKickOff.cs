// --------------------------------------------------------------------------
// <copyright file="BetTransEarlyKickOff.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.24">Create</change>
//   <change who="Kaden.Trinh" date="2013.10.23">Refactor code. Move from sportsbook library.</change>
// </history>

namespace Fanex.TicketControl.Core.DTO
{
    /// <summary>
    /// Bet Trans Early KickOff.
    /// </summary>
    public class BetTransEarlyKickOff : BetTrans
    {
        public long SubTransId { get; set; }

        public short ChoiceBetType { get; set; }

        public bool IsMP { get; set; }

        public string RefNo { get; set; }

        public decimal MOdds { get; set; }

        public string ChoiceBetTeam { get; set; }

        public string ChoiceStatus { get; set; }
    }
}