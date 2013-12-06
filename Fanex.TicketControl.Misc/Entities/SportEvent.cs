// --------------------------------------------------------------------------
// <copyright file="SportEvent.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Entities
{
    using System;

    /// <summary>
    /// Sport event.
    /// </summary>
    [Serializable]
    public class SportEvent
    {
        public string BetType
        {
            get;
            set;
        }

        public string Choice
        {
            get;
            set;
        }

        public string Event
        {
            get;
            set;
        }

        public string Hdp
        {
            get;
            set;
        }

        public string LeagueName
        {
            get;
            set;
        }

        public string LiveScore
        {
            get;
            set;
        }

        public string MoreBetInfo
        {
            get;
            set;
        }

        public string Selection
        {
            get;
            set;
        }

        public string SportType
        {
            get;
            set;
        }
    }
}