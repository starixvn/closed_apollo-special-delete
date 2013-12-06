// -----------------------------------------------------------------------
// <copyright file="GetBettypeCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.TicketControl.Core.Criteria
{
    using System.Diagnostics.CodeAnalysis;

    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetBettypeCriteria : CriteriaBase
    {
        public ProductType Mode
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "Core_SelBettype";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}