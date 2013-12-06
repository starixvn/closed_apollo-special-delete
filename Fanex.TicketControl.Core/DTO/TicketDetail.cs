// ----------------------------------------------------------------------
// <copyright file="TicketDetail.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.DTO
{
    using System.Web.Script.Serialization;

    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Ticket detail.
    /// </summary>
    public class TicketDetail
    {
        public string ChoiceHtml
        {
            get;
            set;
        }

        public string OddHtml
        {
            get;
            set;
        }

        [ScriptIgnore]
        public ITicket Ticket
        {
            get;
            set;
        }
    }
}