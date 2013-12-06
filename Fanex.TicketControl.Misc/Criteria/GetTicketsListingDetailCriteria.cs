// --------------------------------------------------------------------------
// <copyright file="GetTicketsListingDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.DTO;

    /// <summary>
    /// Ticket listing criteria.
    /// </summary>
    public class GetTicketsListingDetailCriteria : CriteriaBase
    {
        public GetTicketsListingDetailCriteria()
        {
            BetTypes = string.Empty;
            BetTypesLive = string.Empty;
        }

        [SpParam(Ignored = true)]
        public string AwayName
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public string BetTypeName
        {
            get;
            set;
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

        [SpParam(Ignored = true)]
        public DateTime EventDate
        {
            get;
            set;
        }

        public DateTime? FromTransDate
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public string HomeName
        {
            get;
            set;
        }

        public int? LiveAwayScore
        {
            get;
            set;
        }

        public int? LiveHomeScore
        {
            get;
            set;
        }

        public int LiveType
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public string MatchCode
        {
            get;
            set;
        }

        public int MatchID
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public int PageIndex
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public int PageSize
        {
            get;
            set;
        }

        public int SAFilter
        {
            get;
            set;
        }

        [SpParam(Ignored = true)]
        public string Title
        {
            get;
            set;
        }

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
            return "Acc_Rpt_TicketsListingDetail";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}