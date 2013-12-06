// --------------------------------------------------------------------------
// <copyright file="SubTicket.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.23">Create</change>
// </history>

namespace Fanex.TicketControl.SportBook.DTO
{
    /// <summary>
    /// Sub parlay ticket.
    /// </summary>
    public class SubTicket
    {
        public long SubTransId { get; set; }

        public string Choice { get; set; }
    }
}