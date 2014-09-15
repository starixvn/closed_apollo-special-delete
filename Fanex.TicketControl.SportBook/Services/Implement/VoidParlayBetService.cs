// -----------------------------------------------------------------------
// <copyright file="VoidParlayBetService.cs" company="Microsoft">
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
    using System.Globalization;
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
    public class VoidParlayBetService : ServiceBase, IVoidParlayBetService
    {
        private readonly ICoreService _coreService;
        private readonly IVoidParlayBetRepository _reponsitory;
        private readonly IMessageService _messageService;

        public VoidParlayBetService()
        {
            _coreService = new CoreService();
            _reponsitory = new VoidParlayBetRepository();
            _messageService = new MessageService();
        }

        /// <summary>
        /// Get parlay ticket.
        /// </summary>
        /// <param name="criteria">Criteria parlay.</param>
        /// <returns>List ticket.</returns>
        public TicketViewModel<ParlayTicket> GetParlayBet(GetParlayBetCriteria criteria)
        {
            var model = new TicketViewModel<ParlayTicket>();

            try
            {
                var criteriaMatchInfo = new GetMatchInfoCriteria { MatchId = criteria.MatchId };
                var match = _coreService.GetMatchInfo(criteriaMatchInfo);
                var listParlayTicket = GetListData<BetTransParlay>(criteria);
                model.Match = match;
                model.Tickets = AssignDataOnGroup(listParlayTicket);
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
                    ticket.ChoiceHtml = (ticket.Ticket.StatusId == Status.Void) ? "<div class='strikethrough'>" + choice + "</div>" : choice;

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
        /// Void parlay ticket.
        /// </summary>
        /// <param name="criteria">Criteria parlay.</param>
        /// <param name="voidBetResult">Void Bet Result.</param>
        /// <returns>Return list result.</returns>
        public Result VoidParlayBet(VoidParlayBetCritetia criteria, out VoidBetResult<BetTransParlay> voidBetResult)
        {
            return _reponsitory.VoidParlayBet(criteria, out voidBetResult);
        }

        /// <summary>
        /// Set new value to ticket.
        /// </summary>
        /// <param name="item">Item value.</param>
        /// <returns>Return ticket.</returns>
        private static Ticket SetValue(BetTransParlay item)
        {
            return new Ticket
            {
                BetId = item.TransId,
                BetTypeId = item.BetType,
                TransId = item.SubTransId,
                RefNo = item.RefNo,
                BetTeam = item.BetTeam,
                LiveHomeScore = item.Choice_LiveHomeScore,
                LiveAwayScore = item.Choice_LiveAwayScore,
                IsLive = item.Choice_IsLiveIndicator,
                Stake = item.Stake,
                Odds = item.Odds,
                OddsType = item.OddsType.ToString(CultureInfo.InvariantCulture),
                MasterOdds = item.MOdds,
                Handicap1 = item.Choice_Hdp1,
                Handicap2 = item.Choice_Hdp2,
                Status = item.Status,
                LeagueId = item.Choice_LeagueId,
                SportTypeId = item.Choice_SportType,
                HomeId = item.Choice_HomeId,
                AwayId = item.Choice_AwayId,
            };
        }

        /// <summary>
        /// Ticket detail.
        /// </summary>
        /// <param name="item">Bet trans data.</param>
        /// <returns>Return ticket detail.</returns>
        private static TicketDetail GetTicketDetail(BetTransParlay item)
        {
            var tickeDetail = new TicketDetail();
            var ticketHelper = new TicketHelper();
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            var betTypeId = item.BetType;
            tickeDetail.Ticket = SetValue(item);
            ticketHelper.IsShowOddsWithChoice = true;
            ticketHelper.ShowSystemParlayDetail = false;
            var choice = builder.BuildChoice(tickeDetail.Ticket, ticketHelper, null, false);
            var choiceMp = choice;

            var oddsHtml = builder.BuildOdds(tickeDetail.Ticket, null, funcGetOddsTypeName);
            var oddName = OddsText.GetOddsName(oddsHtml);
            oddName = string.IsNullOrWhiteSpace(oddName)
                ? oddsHtml.Replace("<br />", " ")
                : oddsHtml.Replace("<br />", " (") + ")";
            tickeDetail.OddHtml = oddName;

            tickeDetail.Ticket.Odds = item.Choice_Odds;
            tickeDetail.Ticket.BetTeam = item.Choice_BetTeam;
            tickeDetail.Ticket.BetTypeId = item.Choice_BetType;
            tickeDetail.Ticket.HomeId = item.Choice_HomeId;
            tickeDetail.Ticket.AwayId = item.Choice_AwayId;

            var choiceFavorite = builder.BuildChoiceBlock(tickeDetail.Ticket, ticketHelper, null, false);
            choiceFavorite.ScoreMapIcon = null;
            choiceFavorite.BetType = null;
            choiceFavorite.Match = null;
            choiceFavorite.League = null;
            var subTransId = item.SubTransId.ToString();
            choiceMp = choiceMp.Replace("<br/><br/>", string.Empty);
            var strChoice = choiceMp.Replace("<div id=", "<div id='dvFavorite_" + subTransId + "'>" + choiceFavorite + "</div><div id=");
            strChoice += "<div id='divEvent_" + subTransId + "'></div>";
            tickeDetail.ChoiceHtml = strChoice;

            return tickeDetail;
        }

        /// <summary>
        /// Get group parlay ticket.
        /// </summary>
        /// <param name="tickets">Group ticket data.</param>
        /// <returns>Return view model.</returns>
        private static GroupViewModel<ParlayTicket> AssignDataOnGroup(IEnumerable<BetTransParlay> tickets)
        {
            var index = 1;
            var listTicket = (from item in tickets
                              let ticketDetail = GetTicketDetail(item)
                              select new ParlayTicket
                              {
                                  No = index++,
                                  BetId = item.TransId,
                                  SubTransId = item.SubTransId,
                                  Player = item.Username,
                                  TransDate = item.TransDate.ToString(FormatKey.LongDateFormatWithSlash),
                                  BetType = item.BetType,
                                  CurrencyName = item.CurrencyName,
                                  Status = item.Status,
                                  Score = string.Join("-", item.Choice_LiveHomeScore, item.Choice_LiveAwayScore),
                                  BetTypeName = item.BetTypeName,
                                  Stake = ConvertByBetType.Stake(item.Stake),
                                  Odds = ticketDetail.OddHtml,
                                  Choice = ticketDetail.ChoiceHtml,
                                  IsDelete = true,
                                  ChoiceStatus = item.Choice_Status
                              }).ToList();

            return new GroupViewModel<ParlayTicket> { Tickets = listTicket };
        }
    }
}