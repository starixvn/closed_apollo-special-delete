// ----------------------------------------------------------------------
// <copyright file="TicketListingModel.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.30">Check StyleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Misc.Entities;

    /// <summary>
    /// View model ticket listing.
    /// </summary>
    [Serializable]
    public class TicketListingModel
    {
        public List<TicketListing> Items
        {
            get;
            set;
        }

        public Match MatchInfo
        {
            get;
            set;
        }

        public Result Result
        {
            get;
            set;
        }
    }
}