// -----------------------------------------------------------------------
// <copyright file="IVoidSingleBetService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IVoidSingleBetService
    {
        TicketViewModel<SingleTicket> GetSingleBet(GetSingleBetCriteria criteria);

        Result CheckMatchIDwithRefno(CheckMatchIdWithRefNoCriteria criteria);

        Result VoidSingleBet(VoidSingleBetCritetia criteria, out VoidBetResult<BetTrans> voidBetResult);
    }
}