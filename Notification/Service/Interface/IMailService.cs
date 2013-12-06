// ----------------------------------------------------------------------
// <copyright file="IMailService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/10/20">Created</change>
//   <change who="Kaden.Trinh" date="2013/10/23">Implement function send mail.</change>
// </history>

namespace Fanex.TicketControl.Notification.Service.Interface
{
    using System.Collections.Generic;
    using DTO;
    using Fanex.TicketControl.Core.DTO;
    using ViewModels;

    public interface IMailService<T>
    {
        void SendEmailToGroup(IList<T> allTickets, IList<NegativeBalance> userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel);

        void SendEmailToIT(IList<T> uusTickets, string userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel);

        void SendEmailToCSD(IList<T> realTickets, string userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel);
    }
}