// -----------------------------------------------------------------------
// <copyright file="IVoidByBetIdService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.26">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.ViewModels;

    public interface IVoidByBetIdService
    {
        TicketViewModel<BaseTicket> GetTicket(GetVoidByBetIdCriteria criteria);

        Result VoidTicket(VoidByBetIdCriteria criteria, out VoidBetResult<BetTrans> voidBetResult);
    }
}