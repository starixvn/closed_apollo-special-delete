// --------------------------------------------------------------------------
// <copyright file="VoidEarlyKOBetCritetia.cs" company="Nexcel Solutions Vietnam">
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
    /// Void early kickoff.
    /// </summary>
    public class VoidEarlyKOBetCritetia : CriteriaBase
    {
        private DateTime? _kickOffDate;

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

        public int MatchId { get; set; }

        public int SAFilter { get; set; }

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

        public string IP
        {
            get
            {
                return Fanex.TicketControl.Core.Utils.Helper.GetClientIp();
            }
        }

        [SpParam(Ignored = true)]
        public Message Message { get; set; }

        public override string GetSettingKey()
        {
            return "EarlyKOTicket_BettransDelSpecialDel_EarlyKO";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}