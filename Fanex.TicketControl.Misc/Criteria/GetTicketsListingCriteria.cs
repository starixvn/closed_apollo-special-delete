// --------------------------------------------------------------------------
// <copyright file="GetTicketsListingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.DTO;

    /// <summary>
    /// Ticket listing.
    /// </summary>
    public class GetTicketsListingCriteria : CriteriaBase
    {
        public GetTicketsListingCriteria()
        {
            BetTypes = string.Empty;
            BetTypesLive = string.Empty;
        }

        public string BetTypes
        {
            get;
            set;
        }

        public string BetTypesLive
        {
            get;
            set;
        }

        public int ErrCode
        {
            get;
            set;
        }

        public string ErrMsg
        {
            get;
            set;
        }

        [Display(Name = "From")]
        public DateTime? FromTransDate
        {
            get;
            set;
        }

        [Display(Name = "Away")]
        public int? LiveAwayScore
        {
            get;
            set;
        }

        [Display(Name = "Home")]
        public int? LiveHomeScore
        {
            get;
            set;
        }

        [Display(Name = "Event Type")]
        public int LiveType
        {
            get;
            set;
        }

        [Display(Name = "MatchId")]
        public int MatchID
        {
            get;
            set;
        }

        [Display(Name = "Ticket Type")]
        public int SAFilter
        {
            get;
            set;
        }

        [Display(Name = "To")]
        public DateTime? ToTransDate
        {
            get;
            set;
        }

        public ProductType Type
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "Acc_Rpt_TicketsListing";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}