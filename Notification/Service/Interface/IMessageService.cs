// ----------------------------------------------------------------------
// <copyright file="IMessageService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>

namespace Fanex.TicketControl.Notification.Service.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Notification.DTO;

    public interface IMessageService
    {
        void BatchMessageSendPrivateMessage(Message message, IList<CreditDeletedBet> normalTranses);

        void SendPrivateMessage(Message message, IList<CreditDeletedBet> normalTranses);

        void SendPrivateMessage(int matchId, Message message, IList<CreditDeletedBet> normalTranses);
    }
}