// -----------------------------------------------------------------------
// <copyright file="VoidEarlyKOBetService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.BetList.Core.Utils;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Resources;
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
    /// TODO: Update summary.
    /// </summary>
    public class VoidEarlyKOBetService : ServiceBase, IVoidEarlyKOBetService
    {
        private readonly ICoreService _coreService;
        private readonly IVoidEarlyKOBetRepository _reponsitory;
        private readonly IMessageService _messageService;

        public VoidEarlyKOBetService()
        {
            _coreService = new CoreService();
            _reponsitory = new VoidEarlyKOBetRepository();
            _messageService = new MessageService();
        }

        /// <summary>
        /// Get early kickoff ticket.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Return value.</returns>
        public TicketViewModel<EarlyKickOffTicket> GetEarlyKOTicket(GetEarlyKOBetCriteria criteria)
        {
            var model = new TicketViewModel<EarlyKickOffTicket>();

            try
            {
                var criteriaMatchInfo = new GetMatchInfoCriteria { MatchId = criteria.MatchId };
                var match = _coreService.GetMatchInfo(criteriaMatchInfo);
                var listEarlyKickOffTicket = GetListData<BetTransEarlyKickOff>(criteria);

                model.Match = match;
                model.Tickets = AssignDataOnGroup(listEarlyKickOffTicket);
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return model;
        }

        /// <summary>
        /// Get sub ticket.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Return list ticket.</returns>
        public List<SubTicket> GetParlaySubTicket(GetParlaySubTicketCriteria criteria)
        {
            var subTickets = new List<SubTicket>();
            try
            {
                var listSubTickets = GetListData<BetTrans>(criteria);
                foreach (var row in listSubTickets)
                {
                    var ticket = new TicketDetail
                    {
                        Ticket = NewTicketParlay(row)
                    };

                    var ticketHelper = new TicketHelper();
                    var factory = new BetListHTMLFactory();
                    var builder = factory.CreateBuilder();
                    ticketHelper.IsShowOddsWithChoice = true;
                    var choice = builder.BuildChoice(ticket.Ticket, ticketHelper, null, false);
                    ticket.ChoiceHtml = (ticket.Ticket.StatusId == 102) ? "<div class='strikethrough'>" + choice + "</div>" : choice;

                    subTickets.Add(new SubTicket
                    {
                        SubTransId = row.TransId,
                        Choice = ticket.ChoiceHtml
                    });
                }
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return subTickets;
        }

        /// <summary>
        /// Void ticket kickoff early.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <param name="voidBetResult">Void bet result.</param>
        /// <returns>Return result.</returns>
        public Result VoidEarlyKOBet(VoidEarlyKOBetCritetia criteria, out VoidBetResult<BetTransEarlyKickOff> voidBetResult)
        {
            return _reponsitory.VoidEarlyKOBet(criteria, out voidBetResult);
        }

        /// <summary>
        /// Ticket detail.
        /// </summary>
        /// <param name="item">Data row value.</param>
        /// <returns>Return ticket detail.</returns>
        private TicketDetail GetTicketDetail(BetTransEarlyKickOff item)
        {
            var tickeDetail = new TicketDetail();
            var ticketHelper = new TicketHelper();
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;

            var betTypeId = item.BetType;

            tickeDetail.Ticket = NewTicket(item);
            ticketHelper.ShowBetType = false;
            ticketHelper.ShowLeague = false;
            ticketHelper.ShowMatch = false;
            ticketHelper.ShowSystemParlayDetail = false;
            ticketHelper.IsShowOddsWithChoice = true;

            var choice = builder.BuildChoice(tickeDetail.Ticket, ticketHelper, null, false).Replace("<br/><br/>", string.Empty);

            var oddsHtml = builder.BuildOdds(tickeDetail.Ticket, null, funcGetOddsTypeName);
            var oddName = OddsText.GetOddsName(oddsHtml);
            oddName = string.IsNullOrWhiteSpace(oddName)
                ? oddsHtml.Replace("<br />", " ")
                : oddsHtml.Replace("<br />", " (") + ")";
            tickeDetail.OddHtml = oddName;

            var strChoiceFavorite = string.Empty;
            if (item.IsMP)
            {
                tickeDetail.Ticket.Odds = item.Odds;
                tickeDetail.Ticket.BetTeam = item.BetTeam;
                tickeDetail.Ticket.BetTypeId = item.BetType;
                tickeDetail.Ticket.HomeId = item.HomeId;
                tickeDetail.Ticket.AwayId = item.AwayId;

                var choiceFavorite = builder.BuildChoiceBlock(tickeDetail.Ticket, ticketHelper, null, false);

                choiceFavorite.ScoreMapIcon = null;
                choiceFavorite.BetType = null;
                choiceFavorite.Match = null;
                choiceFavorite.League = null;
                strChoiceFavorite = choiceFavorite;
            }

            var strChoice = choice.Replace("<div id=", "<div id='dvFavorite_" + item.SubTransId + "'>" + strChoiceFavorite + "</div><div id=");
            strChoice += "<div id='divEvent_" + item.SubTransId + "'></div>";
            tickeDetail.ChoiceHtml = strChoice;
            return tickeDetail;
        }

        /// <summary>
        /// New ticket detail.
        /// </summary>
        /// <param name="item">Data row parameter.</param>
        /// <returns>Return ticket.</returns>
        private Ticket NewTicket(BetTransEarlyKickOff item)
        {
            return new Ticket()
            {
                BetId = item.TransId,
                BetTypeId = item.IsMP ? item.ParlayType : item.BetType,
                TransId = item.SubTransId,
                RefNo = item.RefNo,
                BetTeam = item.BetTeam,
                LiveHomeScore = item.LiveHomeScore,
                LiveAwayScore = item.LiveAwayScore,
                Stake = item.Stake,
                Odds = item.Odds,
                OddsType = item.OddsType.ToString(),
                MasterOdds = item.MOdds,
                Handicap1 = item.Hdp1,
                Handicap2 = item.Hdp2,
                Status = item.Status,
                HomeId = item.HomeId,
                AwayId = item.AwayId,
                LeagueId = item.LeagueId,
                SportTypeId = item.SportType,
            };
        }

        /// <summary>
        /// Assign data.
        /// </summary>
        /// <param name="earlyTickets">Data value.</param>
        /// <returns>Return html value.</returns>
        private GroupViewModel<EarlyKickOffTicket> AssignDataOnGroup(IEnumerable<BetTransEarlyKickOff> earlyTickets)
        {
            var index = 1;
            var tickets = (from item in earlyTickets
                           let ticketDetail = GetTicketDetail(item)
                           select new EarlyKickOffTicket
                           {
                               No = index++,
                               BetId = item.TransId,
                               SubTransId = item.SubTransId,
                               Player = item.Username,
                               TransDate = item.TransDate.ToString(FormatKey.LongDateFormatWithSlash),
                               BetType = item.BetType,
                               CurrencyName = item.CurrencyName,
                               Status = item.Status,
                               Score = string.Join("-", item.LiveHomeScore, item.LiveAwayScore),
                               IsDelete = true,
                               BetTypeName = item.BetTypeName,
                               Stake = ConvertByBetType.Stake(item.Stake),
                               Odds = ticketDetail.OddHtml,
                               Choice = ticketDetail.ChoiceHtml
                           }).ToList();
            return new GroupViewModel<EarlyKickOffTicket> { Tickets = tickets };
        }
    }
}