// --------------------------------------------------------------------------
// <copyright file="IRecoverBetService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.ViewModels;

    public interface IRecoverBetService
    {
        TicketViewModel<SingleTicket> GetTicketRecover(GetRecoverBetCriteria criteria);

        Result RecoverBet(RecoverBetCriteria criteria, out VoidBetResult<BetTrans> voidBetResult);
    }
}