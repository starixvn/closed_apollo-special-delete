// -----------------------------------------------------------------------
// <copyright file="GetCurrencyCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.TicketControl.Core.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetCurrencyCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Core_ExchangeCurrencySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}