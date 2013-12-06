// -----------------------------------------------------------------------
// <copyright file="VoidByBetIdService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.26">Implement function void bet by BetId</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.BetList.Core.Utils;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Utils;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.Repositories.Impl;
    using Fanex.TicketControl.SportBook.Repositories.Interface;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.ViewModels;
    using CacheRef = BetList.Core.TicketHelper.CacheRef;

    /// <summary>
    /// Service void by bet id.
    /// </summary>
    public class VoidByBetIdService : ServiceBase, IVoidByBetIdService
    {
        private readonly IVoidByBetIdRepository _reponsitory;

        public VoidByBetIdService(IVoidByBetIdRepository voidByBetIdRepository = null)
        {
            _reponsitory = voidByBetIdRepository ?? new VoidByBetIdRepository();
        }

        /// <summary>
        /// Get void ticket list by bet id.
        /// </summary>
        /// <param name="criteria">Void bet Criteria.</param>
        /// <returns>Return list ticket.</returns>
        public TicketViewModel<BaseTicket> GetTicket(GetVoidByBetIdCriteria criteria)
        {
            var model = new TicketViewModel<BaseTicket>();

            try
            {
                var listTicket = GetListData<BetTrans>(criteria);
                model.Tickets = AssignDataOnGroup(listTicket);
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return model;
        }

        /// <summary>
        /// Void bet ticket.
        /// </summary>
        /// <param name="criteria">Void Bet criteria.</param>
        /// <param name="voidBetResult">List result void bet.</param>
        /// <returns>Return result.</returns>
        public Result VoidTicket(VoidByBetIdCriteria criteria, out VoidBetResult<BetTrans> voidBetResult)
        {
            return _reponsitory.VoidTicket(criteria, out voidBetResult);
        }

        #region Utils

        /// <summary>
        /// Ticket detail.
        /// </summary>
        /// <param name="item">Bet trans data.</param>
        /// <returns>Return ticket detail.</returns>
        private static TicketDetail GetTicketDetail(BetTrans item)
        {
            var tickeDetail = new TicketDetail();
            var ticketHelper = new TicketHelper();
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;

            var betTypeId = item.BetType;

            tickeDetail.Ticket = new Ticket
            {
                BetId = item.TransId,
                BetTypeId = betTypeId,
                BetTeam = item.BetTeam,
                LiveHomeScore = item.LiveHomeScore,
                LiveAwayScore = item.LiveAwayScore,
                IsLive = item.IsLiveIndicator,
                Stake = item.Stake,
                Odds = item.Odds,
                OddsType = item.OddsType.ToString(CultureInfo.InvariantCulture),
                Handicap1 = item.Hdp1,
                Handicap2 = item.Hdp2,
                Status = item.Status,
                HomeId = item.HomeId,
                AwayId = item.AwayId,
                Winlost = item.WinLost,
                LeagueId = item.LeagueId,
                SportTypeId = item.SportType
            };

            if (betTypeId == BetTypeConstant.MixParlay || betTypeId == BetTypeConstant.SystemParlay)
            {
                ticketHelper.IsShowOddsWithChoice = true;
            }

            tickeDetail.ChoiceHtml = builder.BuildChoice(tickeDetail.Ticket, ticketHelper, null, false);
            var oddsHtml = builder.BuildOdds(tickeDetail.Ticket, null, funcGetOddsTypeName);
            var oddName = OddsText.GetOddsName(oddsHtml);
            oddName = string.IsNullOrWhiteSpace(oddName)
                ? oddsHtml.Replace("<br />", " ")
                : oddsHtml.Replace("<br />", " (") + ")";
            tickeDetail.OddHtml = oddName;

            return tickeDetail;
        }

        /// <summary>
        /// Get group ticket.
        /// </summary>
        /// <param name="tickets">Group ticket data.</param>
        /// <returns>Return view model.</returns>
        private static GroupViewModel<BaseTicket> AssignDataOnGroup(IEnumerable<BetTrans> tickets)
        {
            var index = 1;
            var listTicket = (from item in tickets
                              let ticketDetail = GetTicketDetail(item)
                              select new BaseTicket
                              {
                                  No = index++,
                                  BetId = item.TransId,
                                  Player = item.Username,
                                  TransDate = item.TransDate.ToString(FormatKey.LongDateFormatWithSlash),
                                  BetType = item.BetType,
                                  Handicap = item.Hdp1,
                                  Stake = ConvertByBetType.Stake(item.Stake),
                                  Odds = ticketDetail.OddHtml,
                                  WinLost = item.WinLost,
                                  Status = item.Status,
                                  Score = string.Join("-", item.LiveHomeScore, item.LiveAwayScore),
                                  SecondAcceptFlag = item.SecAcceptFlag ? "<span class='icon-tick'></span>" : string.Empty,
                                  BetTypeName = item.BetTypeName,
                                  Choice = ticketDetail.ChoiceHtml,
                              }).ToList();

            return new GroupViewModel<BaseTicket> { Tickets = listTicket };
        }

        #endregion Utils
    }
}