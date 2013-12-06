// --------------------------------------------------------------------------
// <copyright file="IRecoverBetRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.SportBook.Repositories.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.ViewModels;

    public interface IRecoverBetRepository
    {
        Group<BetTrans> GetTicketRecover(GetRecoverBetCriteria criteria, ref Result result);

        Result RecoverBet(RecoverBetCriteria criteria, out VoidBetResult<BetTrans> voidBetResult);
    }
}