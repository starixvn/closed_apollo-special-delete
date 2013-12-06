// --------------------------------------------------------------------------
// <copyright file="Group.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.05">Create</change>
// </history>

namespace Fanex.TicketControl.SportBook.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Group name.
    /// </summary>
    /// <typeparam name="T">Type value.</typeparam>
    public class Group<T>
    {
        public Group()
        {
            Tickets = new List<T>();
            Name = string.Empty;
        }

        public string Name { get; set; }

        public List<T> Tickets { get; set; }
    }
}