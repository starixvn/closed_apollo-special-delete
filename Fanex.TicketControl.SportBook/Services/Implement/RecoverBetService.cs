// --------------------------------------------------------------------------
// <copyright file="RecoverBetService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Implement
{
    using System;
    using System.Collections.Generic;
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
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.Notification.Service.Impl;
    using Fanex.TicketControl.Notification.Service.Interface;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.Repositories.Impl;
    using Fanex.TicketControl.SportBook.Repositories.Interface;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.ViewModels;
    using CacheRef = BetList.Core.TicketHelper.CacheRef;

    /// <summary>
    /// Recover Bet Service.
    /// </summary>
    public class RecoverBetService : IRecoverBetService
    {
        private readonly ICoreService _coreService;
        private readonly IRecoverBetRepository _repository;
        private readonly IMessageService _messageService;

        public RecoverBetService()
        {
            _coreService = new CoreService();
            _repository = new RecoverBetRepository();
            _messageService = new MessageService();
        }

        /// <summary>
        /// Get recover ticket list.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Return list ticket.</returns>
        public TicketViewModel<SingleTicket> GetTicketRecover(GetRecoverBetCriteria criteria)
        {
            var model = new TicketViewModel<SingleTicket>();
            try
            {
                var result = new Result();
                var matchInfoCriteria = new GetMatchInfoCriteria() { TransId = criteria.TransId };
                var match = _coreService.GetMatchInfo(matchInfoCriteria);
                var listTicket = _repository.GetTicketRecover(criteria, ref result);

                if (match != null)
                {
                    model.Match = match;
                    model.Tickets = AssignDataOnGroup(listTicket, match.HomeId, match.AwayId);
                }

                model.Result = result;
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return model;
        }

        /// <summary>
        /// Recover ticket.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <param name="voidBetResult">Void Bet Result.</param>
        /// <returns>Return result.</returns>
        public Result RecoverBet(RecoverBetCriteria criteria, out VoidBetResult<BetTrans> voidBetResult)
        {
            return _repository.RecoverBet(criteria, out voidBetResult);
        }

        /// <summary>
        /// Get group ticket.
        /// </summary>
        /// <param name="groupTicket">Group ticket data.</param>
        /// <param name="homeId">Home id value.</param>
        /// <param name="awayId">Away id value.</param>
        /// <returns>Return view model.</returns>
        private static GroupViewModel<SingleTicket> AssignDataOnGroup(Group<BetTrans> groupTicket, long homeId, long awayId)
        {
            var index = 1;
            var tickets = (from item in groupTicket.Tickets
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

            return new GroupViewModel<SingleTicket> { Name = groupTicket.Name, Tickets = tickets };
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
                OddsType = item.OddsType.ToString(),
                Handicap1 = item.Hdp1,
                Handicap2 = item.Hdp2,
                Status = item.Status,
                HomeId = homeId,
                AwayId = awayId,
                LeagueId = item.LeagueId,
                SportTypeId = item.SportType
            };

            ticketHelper.IsShowOddsWithChoice = betTypeId == BetTypeConstant.MixParlay || betTypeId == BetTypeConstant.SystemParlay;
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
    }
}