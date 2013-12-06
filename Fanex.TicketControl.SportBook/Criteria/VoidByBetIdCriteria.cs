// --------------------------------------------------------------------------
// <copyright file="VoidByBetIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.27">Create</change>
// </history>

namespace Fanex.TicketControl.SportBook.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.DTO;

    /// <summary>
    /// Criteria void by bet id.
    /// </summary>
    public class VoidByBetIdCriteria : CriteriaBase
    {
        public VoidByBetIdCriteria()
        {
            Message = new Message();
        }

        public string TransIds { get; set; }

        public bool IsDeleteAll { get; set; }

        public string UserPwd { get; set; }

        public int UserID { get; set; }

        public string ErrMsg { get; set; }

        public int ErrCode { get; set; }

        [SpParam(Ignored = true)]
        public Message Message { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BettransDelBet";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}