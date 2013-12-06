// --------------------------------------------------------------------------
// <copyright file="RecoverBetRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>

namespace Fanex.TicketControl.SportBook.Repositories.Impl
{
    using System.Linq;
    using Fanex.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Repository.Impl;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.Repositories.Interface;

    /// <summary>
    /// Recover ticket repository.
    /// </summary>
    public class RecoverBetRepository : BaseRepository, IRecoverBetRepository
    {
        /// <summary>
        /// Get ticket recover.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <param name="result">Output result.</param>
        /// <returns>Return group ticket.</returns>
        public Group<BetTrans> GetTicketRecover(GetRecoverBetCriteria criteria, ref Result result)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var tickets = objectDb.Query<BetTrans>(criteria).ToList();
                result = GetResultInfo(objectDb);
                return new Group<BetTrans>()
                {
                    Tickets = tickets
                };
            }
        }

        /// <summary>
        /// Recover ticket.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <param name="voidBetResult">Output void Bet Result.</param>
        /// <returns>Return result.</returns>
        public Result RecoverBet(RecoverBetCriteria criteria, out VoidBetResult<BetTrans> voidBetResult)
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