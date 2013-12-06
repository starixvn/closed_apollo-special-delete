// -----------------------------------------------------------------------
// <copyright file="IVoidByBetIdRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.27">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;

    public interface IVoidByBetIdRepository
    {
        Result VoidTicket(VoidByBetIdCriteria criteria, out VoidBetResult<BetTrans> voidBetResult);
    }
}