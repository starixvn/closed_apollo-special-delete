// --------------------------------------------------------------------------
// <copyright file="BetTrans.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.20">Create</change>
// </history>

namespace Fanex.TicketControl.Core.DTO
{
    using System;
    using Services.Impl;

    /// <summary>
    /// DTO Bet-trans.
    /// </summary>
    public class BetTrans
    {
        private readonly string _betTypeName = string.Empty;

        public long TransId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal Odds { get; set; }

        public byte OddsType { get; set; }

        public byte StatusId { get; set; }

        public DateTime TransDate { get; set; }

        public short BetType { get; set; }

        public string BetTypeName
        {
            get
            {
                return string.IsNullOrWhiteSpace(_betTypeName) ? CacheRef.GetBettype(BetType) : _betTypeName;
            }
        }

        public string BetTeam { get; set; }

        public decimal Hdp1 { get; set; }

        public decimal Hdp2 { get; set; }

        public bool IsLiveIndicator { get; set; }

        public bool IsNeutral { get; set; }

        public short LiveHomeScore { get; set; }

        public short LiveAwayScore { get; set; }

        public decimal Stake { get; set; }

        public string CurrencyName { get; set; }

        public string Status { get; set; }

        public bool SecAcceptFlag { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public short SportType { get; set; }

        public int LeagueId { get; set; }

        public string Creator { get; set; }

        public decimal WinLost { get; set; }
    }
}