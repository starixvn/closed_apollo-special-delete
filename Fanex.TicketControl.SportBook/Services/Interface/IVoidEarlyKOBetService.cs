// -----------------------------------------------------------------------
// <copyright file="IVoidEarlyKOBetService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kade.Trinh" date="2013.2.05">Created</change>
//   <change who="Kade.Trinh" date="2013.10.24">Refactor code output data after void early.</change>
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
    public interface IVoidEarlyKOBetService
    {
        TicketViewModel<EarlyKickOffTicket> GetEarlyKOTicket(GetEarlyKOBetCriteria criteria);

        List<SubTicket> GetParlaySubTicket(GetParlaySubTicketCriteria criteria);

        Result VoidEarlyKOBet(VoidEarlyKOBetCritetia criteria, out VoidBetResult<BetTransEarlyKickOff> voidBetResult);
    }
}