// ----------------------------------------------------------------------
// <copyright file="VoidedBetViewModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>

namespace Fanex.TicketControl.Notification.ViewModels
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VoidedBetViewModel
    {
        public int No { get; set; }

        public string Username { get; set; }

        public string Odds { get; set; }

        public string OddsType { get; set; }

        public DateTime TransDate { get; set; }

        public string BetType { get; set; }

        public string BetTeam { get; set; }

        public long BetId { get; set; }

        public string Handicap { get; set; }

        public decimal LiveHomeScore { get; set; }

        public decimal LiveAwayScore { get; set; }

        public string Stake { get; set; }

        public string CurrencyName { get; set; }

        public string SecAcceptFlag { get; set; }

        public string Creator { get; set; }

        public string Status { get; set; }

        public string Choice { get; set; }

        public decimal WinLost { get; set; }
    }
}