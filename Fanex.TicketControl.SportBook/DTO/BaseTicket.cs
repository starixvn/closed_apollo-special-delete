// --------------------------------------------------------------------------
// <copyright file="BaseTicket.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.SportBook.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseTicket
    {
        public int No { get; set; }

        public long BetId { get; set; }

        public string Player { get; set; }

        public string TransDate { get; set; }

        public int BetType { get; set; }

        public string BetTypeName { get; set; }

        public string Choice { get; set; }

        public string Stake { get; set; }

        public string Odds { get; set; }

        public string CurrencyName { get; set; }

        public string Status { get; set; }

        public decimal Handicap { get; set; }

        public string Score { get; set; }

        public string SecondAcceptFlag { get; set; }

        public string Creator { get; set; }

        public decimal WinLost { get; set; }
    }
}