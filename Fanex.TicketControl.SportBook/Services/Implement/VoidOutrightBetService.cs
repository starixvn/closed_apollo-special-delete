// -----------------------------------------------------------------------
// <copyright file="VoidOutrightBetService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
//   <change who="Kaden.Trinh" date="2013.10.24">Refactor code and Implement GetTicketDetail function.</change>
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
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Repository.Impl;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Utils;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.DTO;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.ViewModels;
    using CacheRef = BetList.Core.TicketHelper.CacheRef;

    /// <summary>
    /// Void outright ticket service.
    /// </summary>
    public class VoidOutrightBetService : ServiceBase, IVoidOutrightBetService
    {
        private readonly BaseRepository _repository;

        public VoidOutrightBetService()
        {
            _repository = new BaseRepository();
        }

        /// <summary>
        /// Get league list.
        /// </summary>
        /// <returns>Return list.</returns>
        public IList<League> GetOutrightLeague()
        {
            IList<League> leagues = new List<League>();
            try
            {
                Func<IDataReader, League> mappingFunc = r =>
                {
                    return new League
                    {
                        Id = Convert.ToString(r["LeagueId"]),
                        Name = CacheRef.GetLeagueName(r["LeagueId"])
                    };
                };

                leagues = _repository.Fetch<League>(new GetOutrightLeagueCriteria(), mappingFunc);
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return leagues;
        }

        /// <summary>
        /// Get team name by league.
        /// </summary>
        /// <param name="criteria">Criteria outright team.</param>
        /// <returns>List team.</returns>
        public IList<Team> GetTeamByLeague(GetOutrightTeamCriteria criteria)
        {
            IList<Team> teams = new List<Team>();
            try
            {
                Func<IDataReader, Team> mappingFunc = r =>
                {
                    return new Team
                    {
                        Id = Convert.ToString(r["orid"]),
                        Name = CacheRef.GetTeamName(r["teamid"])
                    };
                };

                teams = _repository.Fetch<Team>(criteria, mappingFunc);
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return teams;
        }

        /// <summary>
        /// Get outright ticket.
        /// </summary>
        /// <param name="criteria">Criteria parlay outright ticket.</param>
        /// <returns>Return list ticket.</returns>
        public TicketViewModel<OutrightTicket> GetOutrightBet(GetOutrightBetCriteria criteria)
        {
            var model = new TicketViewModel<OutrightTicket>();
            try
            {
                var listOurightTicket = GetListData<BetTrans>(criteria);
                model.Tickets = AssignDataOnGroup(listOurightTicket);
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return model;
        }

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
                Stake = item.Stake,
                Odds = item.Odds,
                OddsType = item.OddsType.ToString(CultureInfo.InvariantCulture),
                Handicap1 = item.Hdp1,
                Handicap2 = item.Hdp2,
                Status = item.Status,
            };

            var oddsHtml = builder.BuildOdds(tickeDetail.Ticket, null, funcGetOddsTypeName);
            var oddName = OddsText.GetOddsName(oddsHtml);
            oddName = string.IsNullOrWhiteSpace(oddName)
                ? oddsHtml.Replace("<br />", " ")
                : oddsHtml.Replace("<br />", " (") + ")";
            tickeDetail.OddHtml = oddName;

            return tickeDetail;
        }

        /// <summary>
        /// Assign data on group.
        /// </summary>
        /// <param name="tickets">List ticket.</param>
        /// <returns>Return group list ticket.</returns>
        private GroupViewModel<OutrightTicket> AssignDataOnGroup(IEnumerable<BetTrans> tickets)
        {
            var index = 1;
            var listTicket = (from item in tickets
                              let ticketDetail = GetTicketDetail(item)
                              select new OutrightTicket
                              {
                                  No = index++,
                                  BetId = item.TransId,
                                  Player = item.Username,
                                  TransDate = item.TransDate.ToString(FormatKey.LongDateFormatWithSlash),
                                  Stake = ConvertByBetType.Stake(item.Stake),
                                  Odds = ticketDetail.OddHtml,
                                  CurrencyName = item.CurrencyName,
                                  Status = item.Status
                              }).ToList();

            return new GroupViewModel<OutrightTicket> { Tickets = listTicket };
        }
    }
}