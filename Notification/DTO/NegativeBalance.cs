// <copyright file="NegativeBalance.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// <history>
//   <change who="Kaden.Trinh" date="2013.10.22">Object Negative Balance.</change>
// </history>

namespace Fanex.TicketControl.Notification.DTO
{
    /// <summary>
    /// Negative Balance Customers.
    /// </summary>
    public class NegativeBalance
    {
        public string Username { get; set; }

        public decimal Balance { get; set; }

        public string CurrencyName { get; set; }
    }
}