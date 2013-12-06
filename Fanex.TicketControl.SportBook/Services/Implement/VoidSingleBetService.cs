// -----------------------------------------------------------------------
// <copyright file="VoidSingleBetService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.BetList.Core.Utils;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Utils;
    using Fanex.TicketControl.Misc.Repositories.Impl;
    using Fanex.TicketControl.Misc.Repositories.Interface;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.Notification.Service.Impl;
    using Fanex.TicketControl.Notification.Service.Interface;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.ViewModels;
    using CacheRef = BetList.Core.TicketHelper.CacheRef;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VoidSingleBetService : ServiceBase, IVoidSingleBetService
    {
        private readonly ICoreService _coreService;
        private readonly IVoidSingleBetRepository _reponsitory;
        private readonly IMessageService _messageService;

        public VoidSingleBetService()
        {
            _coreService = new CoreService();
            _reponsitory = new VoidSingleBetRepository();
            _messageService = new MessageService();
        }

        /// <summary>
        /// Get void ticket list by group customer.
        /// </summary>
        /// <param name="criteria">Get Single Ticket Criteria.</param>
        /// <returns>Return list ticket.</returns>
        public TicketViewModel<SingleTicket> GetSingleBet(GetSingleBetCriteria criteria)
        {
            var model = new TicketViewModel<SingleTicket>();
            try
            {
                var criteriaMatchInfo = new GetMatchInfoCriteria { MatchId = criteria.MatchId };
                var match = _coreService.GetMatchInfo(criteriaMatchInfo);
                var listTicket = GetListData<BetTrans>(criteria);

                model.Match = match;
                model.Tickets = AssignDataOnGroup(listTicket, match.HomeId, match.AwayId);
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return model;
        }

        /// <summary>
        /// Void single bet ticket.
        /// </summary>
        /// <param name="criteria">Void Single Ticket criteria.</param>
        /// <param name="voidBetResult">Void Bet Result.</param>
        /// <returns>Return result.</returns>
        public Result VoidSingleBet(VoidSingleBetCritetia criteria, out VoidBetResult<BetTrans> voidBetResult)
        {
            return _reponsitory.VoidSingleBet(criteria, out voidBetResult);
        }

        /// <summary>
        /// Check match is exists.
        /// </summary>
        /// <param name="criteria">Criteria info.</param>
        /// <returns>Return result.</returns>
        public Result CheckMatchIDwithRefno(CheckMatchIdWithRefNoCriteria criteria)
        {
            var result = new Result();
            try
            {
                criteria.Type = SportType.Sportsbook;
                result = _coreService.CheckMatchIdWithRefno(criteria);
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return result;
        }

        /// <summary>
        /// Ticket detail.
        /// </summary>
        /// <param name="item">Bet trans data.</param>
        /// <param name="homeId">Home id value.</param>
        /// <param name="awayId">Away id value.</param>
        /// <returns>Return ticket detail.</returns>
        private static TicketDetail GetTicketDetail(BetTrans item, long homeId, long awayId)
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
                HomeId = homeId,
                AwayId = awayId,
                LeagueId = item.LeagueId,
                SportTypeId = item.SportType
            };

            ticketHelper.ShowBetType = false;
            ticketHelper.ShowLeague = false;
            ticketHelper.ShowMatch = false;
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
        /// <param name="homeId">Home id value.</param>
        /// <param name="awayId">Away id value.</param>
        /// <returns>Return view model.</returns>
        private static GroupViewModel<SingleTicket> AssignDataOnGroup(IEnumerable<BetTrans> tickets, long homeId, long awayId)
        {
            var index = 1;
            var listTicket = (from item in tickets
                              let ticketDetail = GetTicketDetail(item, homeId, awayId)
                              select new SingleTicket
                              {
                                  No = index++,
                                  BetId = item.TransId,
                                  Player = item.Username,
                                  TransDate = item.TransDate.ToString(FormatKey.LongDateFormatWithSlash),
                                  BetType = item.BetType,
                                  Handicap = item.Hdp1,
                                  Stake = ConvertByBetType.Stake(item.Stake),
                                  Odds = ticketDetail.OddHtml,
                                  CurrencyName = item.CurrencyName,
                                  Status = item.Status,
                                  Score = string.Join("-", item.LiveHomeScore, item.LiveAwayScore),
                                  SecondAcceptFlag = item.SecAcceptFlag ? "<span class='icon-tick'></span>" : string.Empty,
                                  IsDelete = true,
                                  BetTypeName = item.BetTypeName,
                                  Choice = ticketDetail.ChoiceHtml
                              }).ToList();

            return new GroupViewModel<SingleTicket> { Tickets = listTicket };
        }
    }
}