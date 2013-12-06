// --------------------------------------------------------------------------
// <copyright file="BatchMessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Resources;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Misc.Criteria;
    using Fanex.TicketControl.Misc.Repositories.Impl;
    using Fanex.TicketControl.Misc.Repositories.Interface;
    using Fanex.TicketControl.Misc.Services.Interface;
    using Fanex.TicketControl.Notification.Service.Impl;
    using Fanex.TicketControl.Notification.Service.Interface;

    /// <summary>
    /// Batch message service.
    /// </summary>
    public class BatchMessageService : IBatchMessageService
    {
        private readonly IMessageService _messageService;
        private readonly IBatchMessageRepository _repository;
        private IList<string> _listTransIdFail = new List<string>();

        public BatchMessageService()
        {
            _repository = new BatchMessageRepository();
            _messageService = new MessageService();
        }

        /// <summary>
        /// Update message.
        /// </summary>
        /// <param name="message">Message value.</param>
        /// <param name="transIds">Trans id value.</param>
        /// <returns>Return result.</returns>
        public Result UpdateMessage(Message message, string transIds)
        {
            var results = new Result();
            try
            {
                var listTransIds = transIds.Split(',');

                foreach (string tranid in listTransIds)
                {
                    TransIdsNotExist(tranid.Trim());
                }

                var transIdFail = string.Join(", ", _listTransIdFail);
                if (transIdFail.Length > 0)
                {
                    results.ErrorCode = 1;
                    results.ErrorMsg = transIdFail;
                }
                else
                {
                    var distinctTransIds = string.Join(",", listTransIds.Distinct()).Trim();
                    SendPrivateMessage(message, distinctTransIds);
                    results.ErrorCode = 0;
                    results.ErrorMsg = Messages.UpdateBatchMsgSuccessfully;
                }
            }
            catch (Exception exception)
            {
                LoggingHandler.Log(exception);
            }

            return results;
        }

        /// <summary>
        /// Send Message.
        /// </summary>
        /// <param name="message">Message value.</param>
        /// <param name="transId">Transaction Id.</param>
        private void SendPrivateMessage(Message message, string transId)
        {
            var criteria = new GetBatchMessageCriteria { TransIds = transId };
            var creditDeletedBet = _repository.GetBatchMessage(criteria);

            if (creditDeletedBet.Count > 0)
            {
                _messageService.BatchMessageSendPrivateMessage(message, creditDeletedBet);
            }
        }

        /// <summary>
        /// Get list trans id not exist.
        /// </summary>
        /// <param name="transId">Trans id value.</param>
        /// <returns>Return list string.</returns>
        private IList<string> TransIdsNotExist(string transId)
        {
            var criteria = new GetBatchMessageCriteria { TransIds = transId };
            var creditDeletedBet = _repository.GetBatchMessage(criteria);

            if (creditDeletedBet.Count == 0)
            {
                _listTransIdFail.Add(transId);
            }

            return _listTransIdFail;
        }
    }
}