// ----------------------------------------------------------------------
// <copyright file="MessageRepository.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>

namespace Fanex.TicketControl.Notification.DataAccess.Impl
{
    using System;
    using Core.DTO;
    using Core.Services.Impl;
    using Criteria;
    using Data;
    using Interface;

    /// <summary>
    /// Message Repository.
    /// </summary>
    public class MessageRepository : ServiceBase, IMessageRepository
    {
        /// <summary>
        /// Create Private Message.
        /// </summary>
        /// <param name="criteria">Criteria parameter.</param>
        public void CreatePrivateMessage(CreatePrivateMessageCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.ExecuteNonQuery(criteria.GetSpParams());
            }
        }

        /// <summary>
        /// Create Message.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Return error code.</returns>
        public int CreateMessage(CreateMessageCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = objectDb.ExecuteScalar(criteria.GetSpParams());
                return Convert.ToInt32(result);
            }
        }
    }
}