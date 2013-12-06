// --------------------------------------------------------------------------
// <copyright file="GetVoidByBetIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.26">Create</change>
// </history>

namespace Fanex.TicketControl.SportBook.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.Utils;

    /// <summary>
    /// Void By BetId Ticket Criteria.
    /// </summary>
    public class GetVoidByBetIdCriteria : CriteriaBase
    {
        private DateTime? _fromDate;

        private DateTime? _toDate;

        public DateTime? FromDate
        {
            get
            {
                if (_fromDate.HasValue)
                {
                    _fromDate = Helper.ConvertDatetimeZoneCanada(Convert.ToDateTime(_fromDate));
                }

                return _fromDate;
            }

            set
            {
                _fromDate = value;
            }
        }

        public DateTime? ToDate
        {
            get
            {
                if (_toDate.HasValue)
                {
                    _toDate = Helper.ConvertDatetimeZoneCanada(Convert.ToDateTime(_toDate));
                }

                return _toDate;
            }

            set
            {
                _toDate = value;
            }
        }

        public long TransID { get; set; }

        public string Username { get; set; }

        public bool IsShowAll { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetsSelBetList_ByTransID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}