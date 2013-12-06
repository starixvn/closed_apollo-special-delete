// -----------------------------------------------------------------------
// <copyright file="GetMatchInfoCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.TicketControl.Core.Criteria
{
    using System;

    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMatchInfoCriteria : CriteriaBase
    {
        public int MatchId
        {
            get;
            set;
        }

        public long TransId
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "Core_MatchSelByID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}