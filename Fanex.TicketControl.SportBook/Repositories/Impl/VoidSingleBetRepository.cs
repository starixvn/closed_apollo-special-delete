// --------------------------------------------------------------------------
// <copyright file="VoidSingleBetRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Repository.Impl;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Misc.Repositories.Interface;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;

    /// <summary>
    /// Void Single Ticket Repository.
    /// </summary>
    public class VoidSingleBetRepository : BaseRepository, IVoidSingleBetRepository
    {
        /// <summary>
        /// Void bet result.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <param name="voidBetResult">Output void bet result.</param>
        /// <returns>Return result.</returns>
        public Result VoidSingleBet(VoidSingleBetCritetia criteria, out VoidBetResult<BetTrans> voidBetResult)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multiTable = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    voidBetResult = multiTable.IsNull
                        ? new VoidBetResult<BetTrans>()
                        : new VoidBetResult<BetTrans>
                        {
                            Match = multiTable.Read<Match>().FirstOrDefault(),
                            AllTrans = multiTable.Read<BetTrans>().ToList(),
                            UserNegativeBalance = multiTable.Read<NegativeBalance>().ToList(),
                            ListMessage = multiTable.Read<CreditDeletedBet>().ToList()
                        };
                }

                return GetResultInfo(objectDb);
            }
        }
    }
}