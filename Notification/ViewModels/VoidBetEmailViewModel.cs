// --------------------------------------------------------------------------
// <copyright file="VoidBetEmailViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>

namespace Fanex.TicketControl.Notification.ViewModels
{
    using System.Collections.Generic;
    using Core.DTO;
    using Core.Services.Impl;
    using Fanex.TicketControl.Notification.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VoidBetEmailViewModel
    {
        public string Title { get; set; }

        public Match Match { get; set; }

        public IList<VoidedBetViewModel> Transactions { get; set; }

        public string UserNegativeBalance { get; set; }

        public string ActionType { get; set; }

        public string ActorUsername { get; set; }

        public string[] FunctionName { get; set; }

        public string VoidReason { get; set; }

        public bool IsITGroup { get; set; }

        public bool ShowFullChoice { get; set; }

        public string EmailTemplate { get; set; }
    }
}