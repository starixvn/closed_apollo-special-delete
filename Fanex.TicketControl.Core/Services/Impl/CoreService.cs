// ----------------------------------------------------------------------
// <copyright file="CoreService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Repository.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CoreService : ServiceBase, ICoreService
    {
        private readonly BaseRepository _repository;

        public CoreService()
        {
            _repository = new BaseRepository();
        }

        /// <summary>
        /// Check match exists.
        /// </summary>
        /// <param name="criteria">Criteria check match.</param>
        /// <returns>Return result.</returns>
        public Result CheckMatchIdWithRefno(CheckMatchIdWithRefNoCriteria criteria)
        {
            var result = new Result();
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var ds = objectDb.ExecuteDataSet(criteria.GetSpParams());
                    var hashOutputParams = objectDb.GetOuputParams();

                    result = new Result()
                    {
                        ErrorCode = Convert.ToInt32(hashOutputParams["errcode"]),
                        ErrorMsg = Convert.ToString(hashOutputParams["errmsg"])
                    };
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return result;
        }

        /// <summary>
        /// Get all licensee.
        /// </summary>
        /// <returns>Return list licensee.</returns>
        public IList<Licensee> GetAllLicensees()
        {
            Func<IDataReader, Licensee> mappingFunc = r =>
            {
                return new Licensee
                {
                    Id = r.ToString("LicenseeID"),
                    Name = r.ToString("LicenseeName")
                };
            };

            var licensees = _repository.Fetch<Licensee>(new GetAllLicenseeCriteria(), mappingFunc);
            return licensees;
        }

        /// <summary>
        /// Get bet-type.
        /// </summary>
        /// <param name="criteria">Bet-type criteria.</param>
        /// <returns>Return list bet-types.</returns>
        public IList<Bettypes> GetBettypes(GetBettypeCriteria criteria)
        {
            Func<IDataReader, Bettypes> mappingFunc = r =>
            {
                return new Bettypes
                {
                    Id = r.ToString("typeId"),
                    Name = r.ToString("BetTypeName"),
                    Type = r.ToInt("type"),
                    LiveIndicator = r.ToBool("LiveIndicator")
                };
            };

            IList<Bettypes> bettypes = _repository.Fetch<Bettypes>(criteria, mappingFunc);
            return bettypes;
        }

        /// <summary>
        /// Get list Currency.
        /// </summary>
        /// <returns>Return list currency.</returns>
        public IList<Currency> GetCurrency()
        {
            Func<IDataReader, Currency> mappingFunc = r =>
            {
                return new Currency
                {
                    Id = r.ToString("exchangeid"),
                    Name = r.ToString("currency")
                };
            };

            var obj = _repository.Fetch<Currency>(new GetCurrencyCriteria(), mappingFunc);
            return obj;
        }

        /// <summary>
        /// Get match Info.
        /// </summary>
        /// <param name="criteria">Criteria match.</param>
        /// <returns>Return match info.</returns>
        public Match GetMatchInfo(GetMatchInfoCriteria criteria)
        {
            Func<IDataReader, Match> mappingFunc = r =>
            {
                return new Match
                {
                    MatchId = r.ToInt("MatchId"),
                    HomeId = r.ToInt("HomeId"),
                    AwayId = r.ToInt("AwayId"),
                    MatchCode = r.ToString("MatchCode"),
                    LeagueId = r.ToInt("LeagueId"),
                    IsOutright = r.ToInt("IsOutright") == 1 ? true : false,
                    EventDate = r.ToDateTime("EventDate").ToString(FormatKey.ShortDateFormatWithSlash)
                };
            };

            var matchInfo = _repository.Fetch<Match>(criteria, mappingFunc).FirstOrDefault();
            return matchInfo;
        }
    }
}