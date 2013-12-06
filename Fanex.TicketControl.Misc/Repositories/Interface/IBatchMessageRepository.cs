//----------------------------------------------------------------------------------------------
// <copyright file="IBatchMessageRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Misc.Criteria;
    using Fanex.TicketControl.Notification.DTO;

    public interface IBatchMessageRepository
    {
        IList<CreditDeletedBet> GetBatchMessage(GetBatchMessageCriteria criteria);
    }
}