// --------------------------------------------------------------------------
// <copyright file="GetOutrightTeamCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Criteria outright team.
    /// </summary>
    public class GetOutrightTeamCriteria : CriteriaBase
    {
        public int LeagueId { get; set; }

        public override string GetSettingKey()
        {
            return "OutrightBet_BettransDelSpecial_OutrightSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}