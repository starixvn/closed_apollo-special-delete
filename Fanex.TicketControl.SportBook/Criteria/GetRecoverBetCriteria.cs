// --------------------------------------------------------------------------
// <copyright file="GetRecoverBetCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>

namespace Fanex.TicketControl.SportBook.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Criteria Recover ticket.
    /// </summary>
    public class GetRecoverBetCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "RecoverBet_BettransSelRecoverSpecialBets";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}