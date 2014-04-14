// --------------------------------------------------------------------------
// <copyright file="VoidParlayBetCritetia.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Utils;

    /// <summary>
    /// Criteria Void parlay.
    /// </summary>
    public class VoidParlayBetCritetia : CriteriaBase
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

        public int Type { get; set; }

        public string TransIds { get; set; }

        public string Bettypes { get; set; }

        public string Betteam1378 { get; set; }

        public string Betteam515 { get; set; }

        public string Betteam430 { get; set; }

        public string Betteam6 { get; set; }

        public string Betteam14 { get; set; }

        public string Betteam16 { get; set; }

        public decimal HDP { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string Errmsg { get; set; }

        public int? LiveHomeScore { get; set; }

        public int? LiveAwayScore { get; set; }

        public string IP
        {
            get
            {
                return Helper.GetClientIp();
            }
        }

        [SpParam(Ignored = true)]
        public Message Message { get; set; }

        public override string GetSettingKey()
        {
            return "ParlayTicket_BettransDelSpecialDel_MixParlay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}