//----------------------------------------------------------------------------------------------
// <copyright file="BatchMessageRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.TicketControl.Misc.Criteria;
    using Fanex.TicketControl.Misc.Repositories.Interface;
    using Fanex.TicketControl.Notification.DTO;

    /// <summary>
    /// Batch message repository.
    /// </summary>
    public class BatchMessageRepository : IBatchMessageRepository
    {
        /// <summary>
        /// Get batch message.
        /// </summary>
        /// <param name="criteria">Batch Message Criteria.</param>
        /// <returns>Return value.</returns>
        public IList<CreditDeletedBet> GetBatchMessage(GetBatchMessageCriteria criteria)
        {
            IList<CreditDeletedBet> creditDel;
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                creditDel = objectDb.Query<CreditDeletedBet>(criteria.GetSpParams()).ToList();
            }

            return creditDel;
        }
    }
}