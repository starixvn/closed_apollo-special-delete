// --------------------------------------------------------------------------
// <copyright file="Match.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.27">Create</change>
//   <change who="Kaden.Trinh" date="2013.10.16">Implement mailing list</change>
// </history>
namespace Fanex.TicketControl.Core.DTO
{
    using System;
    using Fanex.TicketControl.Core.Services.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Match
    {
        private readonly string _homeName = string.Empty;
        private readonly string _awayName = string.Empty;
        private readonly string _leagueName = string.Empty;

        public int AwayId
        {
            get;
            set;
        }

        public string AwayName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_awayName) ? CacheRef.GetTeamName(AwayId) : _awayName;
            }
        }

        public string HomeName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_homeName) ? CacheRef.GetTeamName(HomeId) : _homeName;
            }
        }

        public string EventDate
        {
            get;
            set;
        }

        public int HomeId
        {
            get;
            set;
        }

        public string MatchCode
        {
            get;
            set;
        }

        public int MatchId
        {
            get;
            set;
        }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_leagueName) ? CacheRef.GetLeagueName(LeagueId) : _leagueName;
            }
        }

        public bool IsOutright { get; set; }

        public string MatchEvent
        {
            get
            {
                return IsOutright ? string.Join(" - ", HomeName, LeagueName) : string.Join("-vs-", HomeName, AwayName);
            }
        }

        public DateTime DateTime { get; set; }
    }
}