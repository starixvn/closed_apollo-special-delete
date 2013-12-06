// --------------------------------------------------------------------------
// <copyright file="GetSingleBetCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.SportBook.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetSingleBetCriteria : CriteriaBase
    {
        private DateTime? _fromTransdate;

        private DateTime? _toTransdate;

        public DateTime? FromTransdate
        {
            get
            {
                if (_fromTransdate.HasValue)
                {
                    _fromTransdate = Helper.ConvertDatetimeZoneCanada(Convert.ToDateTime(_fromTransdate));
                }

                return _fromTransdate;
            }

            set
            {
                _fromTransdate = value;
            }
        }

        public DateTime? ToTransdate
        {
            get
            {
                if (_toTransdate.HasValue)
                {
                    _toTransdate = Helper.ConvertDatetimeZoneCanada(Convert.ToDateTime(_toTransdate));
                }

                return _toTransdate;
            }

            set
            {
                _toTransdate = value;
            }
        }

        public int MatchId { get; set; }

        public long FromRefNo { get; set; }

        public long ToRefNo { get; set; }

        public decimal Hdp { get; set; }

        public int LiveIndicator { get; set; }

        public int? LiveHomeScore { get; set; }

        public int? LiveAwayScore { get; set; }

        public string Bettypes { get; set; }

        public string Betteam1378 { get; set; }

        public string Betteam515 { get; set; }

        public string Betteam430 { get; set; }

        public string Betteam6 { get; set; }

        public string Betteam14 { get; set; }

        public string Betteam16 { get; set; }

        public string UserName { get; set; }

        public string SAFilter { get; set; }

        public override string GetSettingKey()
        {
            return "SingleTicket_BettransSelSpecialDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}