// --------------------------------------------------------------------------
// <copyright file="IVoidSingleBetRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
//   <change who="Kaden.Trinh" date="2013.10.17">Implement function mailing list.</change>
// </history>

namespace Fanex.TicketControl.Misc.Repositories.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;

    public interface IVoidSingleBetRepository
    {
        Result VoidSingleBet(VoidSingleBetCritetia criteria, out VoidBetResult<BetTrans> voidBetResult);
    }
}