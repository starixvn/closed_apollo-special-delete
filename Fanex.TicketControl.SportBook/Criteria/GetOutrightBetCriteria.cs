// --------------------------------------------------------------------------
// <copyright file="GetOutrightBetCriteria.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Criteria Outright ticket.
    /// </summary>
    public class GetOutrightBetCriteria : CriteriaBase
    {
        public int OutrightId { get; set; }

        public long FromRefNo { get; set; }

        public long ToRefNo { get; set; }

        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "OutrightBet_BettransSelDelSpecial_Outright";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}