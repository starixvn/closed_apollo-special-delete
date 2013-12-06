// ----------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common.Helper;
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Services.Interface;

    /// <summary>
    /// Service base.
    /// </summary>
    public class ServiceBase : IServiceBase
    {
        protected readonly IDynamicRepository DynamicRepository;

        public ServiceBase(IDynamicRepository repository = null)
        {
            DynamicRepository = repository ?? new DynamicRepository();
        }

        protected IDynamicRepository Repository
        {
            get
            {
                return DynamicRepository;
            }
        }

        /// <summary>
        /// Get Report Data with Criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual IEnumerable<T> GetListData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return DynamicRepository.Fetch<T>(criteria);
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return new List<T>();
        }

        /// <summary>
        /// New ticket parlay.
        /// </summary>
        /// <param name="item">Data row parameter.</param>
        /// <returns>Return ticket.</returns>
        protected Ticket NewTicketParlay(BetTrans item)
        {
            return new Ticket()
            {
                BetId = item.TransId,
                BetTypeId = item.BetType,
                TransId = item.TransId,
                BetTeam = item.BetTeam,
                LiveHomeScore = item.LiveHomeScore,
                LiveAwayScore = item.LiveAwayScore,
                Stake = item.Stake,
                Odds = item.Odds,
                Handicap1 = item.Hdp1,
                Handicap2 = item.Hdp2,
                Status = item.Status,
                HomeId = item.HomeId,
                AwayId = item.AwayId,
                LeagueId = item.LeagueId,
                SportTypeId = item.SportType,
                IsNeutral = item.IsNeutral,
                IsLive = item.IsLiveIndicator,
                StatusId = item.StatusId
            };
        }

        /// <summary>
        /// New ticket parlay.
        /// </summary>
        /// <param name="row">Data row parameter.</param>
        /// <returns>Return ticket.</returns>
        protected Ticket NewTicketParlay(DataRow row)
        {
            var bettype = Convert.ToInt32(row["bettype"]);
            var transId = Convert.ToInt64(row["TransId"]);
            var sportType = Convert.ToInt32(row["SportType"]);
            var team = Convert.ToString(row["betteam"]);
            var homeScore = Convert.ToInt32(row["livehomescore"]);
            var awayScore = Convert.ToInt32(row["liveawayscore"]);
            var stake = Convert.ToDecimal(row["StakePositionTaking"]);
            var odds = Convert.ToDecimal(row["odds"]);
            var handicap1 = Convert.ToDecimal(row["hdp1"]);
            var handicap2 = Convert.ToDecimal(row["hdp2"]);
            var homeId = Convert.ToInt64(row["HomeId"]);
            long awayId = Convert.ToInt64(row["AwayId"]);
            var leagueId = Convert.ToInt32(row["LeagueId"]);
            var isNeutral = Convert.ToBoolean(row["IsNeutral"]);
            var isLive = Convert.ToBoolean(row["liveindicator"]);
            var matchCode = Convert.ToString(row["matchcode"]);
            var statusId = Convert.ToInt32(row["statusid"]);

            return new Ticket()
            {
                BetId = transId,
                BetTypeId = bettype,
                TransId = transId,
                BetTeam = team,
                LiveHomeScore = homeScore,
                LiveAwayScore = awayScore,
                Stake = stake,
                Odds = odds,
                Handicap1 = handicap1,
                Handicap2 = handicap2,
                HomeId = homeId,
                AwayId = awayId,
                LeagueId = leagueId,
                SportTypeId = sportType,
                IsNeutral = isNeutral,
                IsLive = isLive,
                MatchCode = matchCode,
                StatusId = statusId
            };
        }
    }
}