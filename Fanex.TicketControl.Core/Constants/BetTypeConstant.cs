// --------------------------------------------------------------------------
// <copyright file="BetTypeConstant.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.09.20">Create</change>
// </history>

namespace Fanex.TicketControl.Core.Constants
{
    /// <summary>
    /// Bet-type constant.
    /// </summary>
    public class BetTypeConstant
    {
        /// <summary>
        /// Mix Parlay.
        /// </summary>
        public const int MixParlay = 9;

        /// <summary>
        /// System Parlay.
        /// </summary>
        public const int SystemParlay = 29;

        /// <summary>
        /// Outright bet type.
        /// </summary>
        public const int Outright = 10;

        /// <summary>
        /// First goal last goal.
        /// </summary>
        public const int Fglg = 14;

        /// <summary>
        /// Half time full time.
        /// </summary>
        public const int Htft = 16;

        /// <summary>
        /// Max number game bet type.
        /// </summary>
        public const int MaxNumberGameBetType = 81;

        /// <summary>
        /// Min number game bet type.
        /// </summary>
        public const int MinNumberGameBetType = 120;

        /// <summary>
        /// Min virtual sports bet type.
        /// </summary>
        public const int MinVirtualSportBetType = 1201;

        /// <summary>
        /// Max virtual sports bet type.
        /// </summary>
        public const int MaxVirtualSportBetType = 1299;

        /// <summary>
        /// Min sporting solution bet type.
        /// </summary>
        public const int MinSportingSolutionBetType = 1301;

        /// <summary>
        /// Max sporting solution bet type.
        /// </summary>
        public const int MaxSportingSolutionBetType = 1399;
    }
}