// ----------------------------------------------------------------------
// <copyright file="CreditDeletedBet.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/10/18">Object data to put message</change>
// </history>
namespace Fanex.TicketControl.Notification.DTO
{
    /// <summary>
    /// List data to put message.
    /// </summary>
    public class CreditDeletedBet
    {
        public long TransId { get; set; }

        public int TargetId { get; set; }

        public string TargetName { get; set; }

        public bool IsAgent { get; set; }

        public int MatchId { get; set; }

        public bool IsOutright { get; set; }
    }
}