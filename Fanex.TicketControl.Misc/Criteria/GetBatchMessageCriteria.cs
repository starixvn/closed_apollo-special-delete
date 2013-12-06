// --------------------------------------------------------------------------
// <copyright file="GetBatchMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get match message.
    /// </summary>
    public class GetBatchMessageCriteria : CriteriaBase
    {
        public string TransIds
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "Misc_BettransSelBet_BatchMsg";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}