// ----------------------------------------------------------------------
// <copyright file="IMessageRepository.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>

namespace Fanex.TicketControl.Notification.DataAccess.Interface
{
    using Core.DTO;
    using Criteria;

    public interface IMessageRepository
    {
        /// <summary>
        ///     Insert target information of the message inserted into table message Target with TargetID.
        /// </summary>
        /// <param name="criteria">Create Private Message Criteria.</param>
        void CreatePrivateMessage(CreatePrivateMessageCriteria criteria);

        /// <summary>
        ///     Insert message into table Message.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Return error code.</returns>
        int CreateMessage(CreateMessageCriteria criteria);
    }
}