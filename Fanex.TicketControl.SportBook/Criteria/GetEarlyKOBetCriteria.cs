// --------------------------------------------------------------------------
// <copyright file="GetEarlyKOBetCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.TicketControl.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetEarlyKOBetCriteria : CriteriaBase
    {
        private DateTime? _kickOffDate;

        public int MatchId { get; set; }

        public string Bettypes { get; set; }

        public decimal Hdp { get; set; }

        public DateTime? KickOffDate
        {
            get
            {
                if (_kickOffDate.HasValue)
                {
                    _kickOffDate = Helper.ConvertDatetimeZoneCanada(Convert.ToDateTime(_kickOffDate));
                }

                return _kickOffDate;
            }

            set
            {
                _kickOffDate = value;
            }
        }

        public string Betteam1378 { get; set; }

        public string Betteam515 { get; set; }

        public string Betteam430 { get; set; }

        public string Betteam6 { get; set; }

        public string Betteam14 { get; set; }

        public string Betteam16 { get; set; }

        public override string GetSettingKey()
        {
            return "EarlyKOTicket_BettransSelSpecialDel_EarlyKO";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}