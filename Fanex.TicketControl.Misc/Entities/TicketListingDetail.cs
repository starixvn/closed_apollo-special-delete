//----------------------------------------------------------------------------------------------
// <copyright file="TicketListingDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Entities
{
    using System;
    using System.Web.Script.Serialization;
    using Fanex.BetList.Core.Entities;
    using Fanex.TicketControl.Core.DTO;

    /// <summary>
    /// Ticket listing detail.
    /// </summary>
    [Serializable]
    public class TicketListingDetail : TicketDetail
    {
        public string AgentWinloss
        {
            get
            {
                if (this.Ticket.Status != "running" && this.Ticket.Status != "waiting")
                {
                    return this.Ticket.Winlost.ToString();
                }

                return string.Empty;
            }
        }

        public long BetId
        {
            get
            {
                return this.Ticket.TransId;
            }
        }

        public string BetType
        {
            get;
            set;
        }

        public int BetTypeId
        {
            get
            {
                return this.Ticket.BetTypeId;
            }
        }

        public string Currency
        {
            get;
            set;
        }

        public bool IsMixParlay
        {
            get;
            set;
        }

        public string Member
        {
            get
            {
                return this.Ticket.UserName;
            }
        }

        public string ParlayBetType
        {
            get;
            set;
        }

        [ScriptIgnore]
        public ITicket ParlayTicket
        {
            get;
            set;
        }

        public bool SAFlag
        {
            get;
            set;
        }

        public string Selection
        {
            get;
            set;
        }

        public decimal Stake
        {
            get
            {
                return this.Ticket.Stake;
            }
        }

        public string Status
        {
            get
            {
                return this.Ticket.Status;
            }
        }

        public string TransDate
        {
            get;
            set;
        }
    }
}