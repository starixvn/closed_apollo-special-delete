// <copyright file="VoidBetResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// <history>
//   <change who="Kaden.Trinh" date="2013.10.19">Implement send mail function.</change>
// </history>
namespace Fanex.TicketControl.Notification.DTO
{
    using System.Collections.Generic;
    using Core.DTO;

    /// <summary>
    /// Void Bet Result.
    /// </summary>
    /// <typeparam name="T">Ticket or Parlay Ticket.</typeparam>
    public class VoidBetResult<T>
    {
        public Match Match { get; set; }

        // The list data deleted to send mail.
        public IList<T> AllTrans { get; set; }

        // The list user negative balance.
        public IList<NegativeBalance> UserNegativeBalance { get; set; }

        // The list data to put private message.
        public IList<CreditDeletedBet> ListMessage { get; set; }
    }
}