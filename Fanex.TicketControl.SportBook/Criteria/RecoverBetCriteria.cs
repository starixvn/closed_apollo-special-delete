// --------------------------------------------------------------------------
// <copyright file="RecoverBetCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.SportBook.Criteria
{
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.DTO;

    /// <summary>
    /// Criteria recover ticket.
    /// </summary>
    public class RecoverBetCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public Message Message { get; set; }

        public override string GetSettingKey()
        {
            return "RecoverBet_BettransRecoverSpecialBets";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}