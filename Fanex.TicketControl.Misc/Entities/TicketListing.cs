// --------------------------------------------------------------------------
// <copyright file="TicketListing.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Entities
{
    using System;
    using Fanex.TicketControl.Core.Services.Impl;

    /// <summary>
    /// Ticket listing.
    /// </summary>
    [Serializable]
    public class TicketListing
    {
        public int BetTypeID
        {
            get;
            set;
        }

        public string BetTypeName
        {
            get
            {
                var betName = CacheRef.GetBettype(BetTypeID);

                switch (BetTypeID)
                {
                    case 86:
                        betName = LiveIndicator ? "Next Odd/Even" : "FT. Odd/Even";
                        break;
                    case 85:
                        betName = "Next Over/Under";
                        break;
                    case 90:
                        betName += LiveIndicator ? " (Live Game)" : " (Pre Game)";
                        break;
                    default:
                        break;
                }

                return betName;
            }
        }

        public bool LiveIndicator
        {
            get;
            set;
        }

        public int TotalTrans
        {
            get;
            set;
        }
    }
}