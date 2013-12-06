// --------------------------------------------------------------------------
// <copyright file="SingleTicket.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.05">Create</change>
// </history>

namespace Fanex.TicketControl.SportBook.DTO
{
    /// <summary>
    /// Single ticket.
    /// </summary>
    public class SingleTicket : BaseTicket
    {
        public bool IsDelete { get; set; }
    }
}