//----------------------------------------------------------------------------------------------
// <copyright file="TicketListingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    using System;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Repository.Impl;
    using Fanex.TicketControl.Misc.Criteria;
    using Fanex.TicketControl.Misc.Entities;
    using Fanex.TicketControl.Misc.Repositories.Interface;
    using Fanex.TicketControl.Misc.ViewModels;

    /// <summary>
    /// Ticket Listing Repository.
    /// </summary>
    public class TicketListingRepository : BaseRepository, ITicketListingRepository
    {
        /// <summary>
        /// Get list ticket.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <returns>Return ticket.</returns>
        public TicketListingModel GetTicketListing(GetTicketsListingCriteria criteria)
        {
            var ticketListingModel = new TicketListingModel();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    // NOTE: Must convert GTM+ 8 into GTM- 4
                    if (criteria.FromTransDate.HasValue && criteria.ToTransDate.HasValue)
                    {
                        criteria.FromTransDate = criteria.FromTransDate.Value.AddHours(-12);
                        criteria.ToTransDate = criteria.ToTransDate.Value.AddHours(-12);
                    }

                    var dataset = objectDb.ExecuteDataSet(criteria.GetSpParams());
                    ticketListingModel.Result = GetResultInfo(objectDb);

                    if (ticketListingModel.Result.ErrorCode == 0)
                    {
                        // get match info
                        var matchTbl = dataset.Tables[0];
                        var matchInfo = new Match();
                        matchInfo.MatchCode = Converter.ToString2(matchTbl.Rows[0]["matchcode"]);
                        matchInfo.EventDate = Converter.ToDateTime(matchTbl.Rows[0]["eventdate"]).ToString("yyyy-MM-dd");
                        matchInfo.HomeId = Convert.ToInt32(matchTbl.Rows[0]["homeid"]);
                        matchInfo.AwayId = Convert.ToInt32(matchTbl.Rows[0]["awayid"]);
                        ticketListingModel.MatchInfo = matchInfo;

                        var mainTable = dataset.Tables[1];
                        var ticketListing = mainTable.AsEnumerable().Select(t => new TicketListing
                        {
                            BetTypeID = Converter.ToInt(t["BetType"]),
                            LiveIndicator = Converter.ToBool(t["LiveIndicator"]),
                            TotalTrans = Converter.ToInt(t["TotalTrans"])
                        }).ToList();

                        ticketListingModel.Items = ticketListing;
                    }
                }
            }

            return ticketListingModel;
        }

        /// <summary>
        /// Get ticket detail.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <returns>Return ticket detail.</returns>
        public DataSet GetTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    // NOTE: Must convert GTM+ 8 into GTM- 4
                    if (criteria.FromTransDate.HasValue && criteria.ToTransDate.HasValue)
                    {
                        criteria.FromTransDate = criteria.FromTransDate.Value.AddHours(-12);
                        criteria.ToTransDate = criteria.ToTransDate.Value.AddHours(-12);
                    }

                    return objectDb.ExecuteDataSet(criteria.GetSpParams());
                }
            }

            return null;
        }
    }
}