// ----------------------------------------------------------------------
// <copyright file="EmailTemplate.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/10/23">Created</change>
// </history>

namespace Fanex.TicketControl.Core.Constants
{
    /// <summary>
    /// Constant template for send mail.
    /// </summary>
    public class EmailTemplate
    {
        /// <summary>
        /// Email template for void single bet.
        /// </summary>
        public const string VoidSingleBet = "Views/EmailTemplate/VoidSingleBet.cshtml";

        /// <summary>
        /// Email template for recover bet.
        /// </summary>
        public const string RecoverBet = "Views/EmailTemplate/RecoverBet.cshtml";

        /// <summary>
        /// Email template for parlay bet.
        /// </summary>
        public const string VoidParlayBet = "Views/EmailTemplate/ParlayBet.cshtml";

        /// <summary>
        /// Email template for void outright bet.
        /// </summary>
        public const string VoidOutrightBet = "Views/EmailTemplate/VoidOutrightBet.cshtml";

        /// <summary>
        /// Email template for void by betID.
        /// </summary>
        public const string VoidByBetID = "Views/EmailTemplate/VoidByBetId.cshtml";

        /// <summary>
        /// Email template for void early kick-off.
        /// </summary>
        public const string VoidEarlyKickOff = "Views/EmailTemplate/VoidEarlyKickOff.cshtml";
    }
}