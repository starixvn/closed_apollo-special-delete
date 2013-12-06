// --------------------------------------------------------------------------
// <copyright file="GetParlaySubTicketCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.SportBook.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetParlaySubTicketCriteria : CriteriaBase
    {
        public string RefNo { get; set; }

        public long TransId { get; set; }

        public int CurrencyId { get; set; }

        public override string GetSettingKey()
        {
            return "ParlayTicket_SubTicket_BettransSelSpecialDel_MixParlay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}