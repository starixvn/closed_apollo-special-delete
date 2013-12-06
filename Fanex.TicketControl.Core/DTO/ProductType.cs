// ----------------------------------------------------------------------
// <copyright file="ProductType.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.DTO
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum ProductType
    {
        /// <summary>
        /// Sports book.
        /// </summary>
        Sportsbook = 0,

        /// <summary>
        /// Number Game.
        /// </summary>
        NumberGame = 1,

        /// <summary>
        /// Mix Parlay.
        /// </summary>
        MixParlay = 2,

        /// <summary>
        /// Ticket Listing.
        /// </summary>
        TicketListing = 3,

        /// <summary>
        /// Early kick off.
        /// </summary>
        EarlyKO = 4
    }
}