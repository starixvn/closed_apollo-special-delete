// -----------------------------------------------------------------------
// <copyright file="VoidByBetIdRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.27">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Repositories.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Repository.Impl;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.Repositories.Interface;

    /// <summary>
    /// DataAccess void by bet id.
    /// </summary>
    public class VoidByBetIdRepository : BaseRepository, IVoidByBetIdRepository
    {
        /// <summary>
        /// Void bet by bet id.
        /// </summary>
        /// <param name="criteria">Criteria void by bet id.</param>
        /// <param name="voidBetResult">Out put list result.</param>
        /// <returns>Return void result.</returns>
        public Result VoidTicket(VoidByBetIdCriteria criteria, out VoidBetResult<BetTrans> voidBetResult)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multiTable = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    voidBetResult = multiTable.IsNull
                       ? new VoidBetResult<BetTrans>()
                       : new VoidBetResult<BetTrans>
                       {
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