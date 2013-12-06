// --------------------------------------------------------------------------
// <copyright file="VoidEarlyKOBetRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
//   <change who="Kaden.Trinh" date="2013.10.24">Output data when void early Kick-off.</change>
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
    /// Void early kickoff.
    /// </summary>
    public class VoidEarlyKOBetRepository : BaseRepository, IVoidEarlyKOBetRepository
    {
        /// <summary>
        /// Void early ticket.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <param name="voidBetResult">Output void bet result.</param>
        /// <returns>Return result.</returns>
        public Result VoidEarlyKOBet(VoidEarlyKOBetCritetia criteria, out VoidBetResult<BetTransEarlyKickOff> voidBetResult)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multiTable = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    voidBetResult = multiTable.IsNull
                        ? new VoidBetResult<BetTransEarlyKickOff>()
                        : new VoidBetResult<BetTransEarlyKickOff>
                        {
                            Match = multiTable.Read<Match>().FirstOrDefault(),
                            AllTrans = multiTable.Read<BetTransEarlyKickOff>().ToList(),
                            UserNegativeBalance = multiTable.Read<NegativeBalance>().ToList(),
                            ListMessage = multiTable.Read<CreditDeletedBet>().ToList()
                        };
                }

                return GetResultInfo(objectDb);
            }
        }
    }
}