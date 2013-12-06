// --------------------------------------------------------------------------
// <copyright file="VoidParlayBetRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
//   <change who="Kaden.Trinh" date="2013.09.23">Refactor code</change>
// </history>

namespace Fanex.TicketControl.SportBook.Repositories.Impl
{
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Repository.Impl;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.Repositories.Interface;

    /// <summary>
    /// Void parlay ticket Repository.
    /// </summary>
    public class VoidParlayBetRepository : BaseRepository, IVoidParlayBetRepository
    {
        /// <summary>
        /// Void parlay ticket.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <param name="voidBetResult">Void bet result.</param>
        /// <returns>Return value.</returns>
        public Result VoidParlayBet(VoidParlayBetCritetia criteria, out VoidBetResult<BetTransParlay> voidBetResult)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multiTable = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    voidBetResult = multiTable.IsNull
                        ? new VoidBetResult<BetTransParlay>()
                        : new VoidBetResult<BetTransParlay>
                        {
                            Match = multiTable.Read<Match>().FirstOrDefault(),
                            AllTrans = multiTable.Read<BetTransParlay>().ToList(),
                            UserNegativeBalance = multiTable.Read<NegativeBalance>().ToList(),
                            ListMessage = multiTable.Read<CreditDeletedBet>().ToList()
                        };
                }

                return GetResultInfo(objectDb);
            }
        }
    }
}