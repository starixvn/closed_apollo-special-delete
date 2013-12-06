// ----------------------------------------------------------------------
// <copyright file="TicketListingBetListModel.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.TicketControl.Misc.Entities;

    /// <summary>
    /// View model detail ticket listing.
    /// </summary>
    [Serializable]
    public class TicketListingBetListModel
    {
        public List<TicketListingDetail> Items
        {
            get;
            set;
        }

        public int TotalItem
        {
            get;
            set;
        }
    }
}