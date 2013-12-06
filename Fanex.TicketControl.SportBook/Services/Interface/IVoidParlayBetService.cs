// -----------------------------------------------------------------------
// <copyright file="IVoidParlayBetService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IVoidParlayBetService
    {
        TicketViewModel<ParlayTicket> GetParlayBet(GetParlayBetCriteria criteria);

        List<SubTicket> GetParlaySubTicket(GetParlaySubTicketCriteria criteria);

        Result VoidParlayBet(VoidParlayBetCritetia criteria, out VoidBetResult<BetTransParlay> voidBetResult);
    }
}