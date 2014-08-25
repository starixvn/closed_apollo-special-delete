// --------------------------------------------------------------------------
// <copyright file="ParlayTicket.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.23">Create</change>
// </history>

namespace Fanex.TicketControl.SportBook.DTO
{
    /// <summary>
    /// Parlay ticket.
    /// </summary>
    public class ParlayTicket : BaseTicket
    {
        public bool IsDelete { get; set; }

        public long SubTransId { get; set; }

        public string ChoiceStatus { get; set; }
    }
}