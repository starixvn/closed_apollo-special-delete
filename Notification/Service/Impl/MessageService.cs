// ----------------------------------------------------------------------
// <copyright file="MessageService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>

namespace Fanex.TicketControl.Notification.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Core.DTO;
    using Criteria;
    using DataAccess.Impl;
    using DataAccess.Interface;
    using DTO;
    using Interface;

    /// <summary>
    /// Message Service.
    /// </summary>
    public class MessageService : IMessageService
    {
        private const int DummyAccountMaxId = 100;

        private readonly IMessageRepository _repository;

        public MessageService()
        {
            _repository = new MessageRepository();
        }

        /// <summary>
        /// Send private message to user.
        /// </summary>
        /// <param name="message">Message id value.</param>
        /// <param name="normalTranses">List trans id.</param>
        public void SendPrivateMessage(Message message, IList<CreditDeletedBet> normalTranses)
        {
            var oldMatchId = 0;
            var messageId = 0;
            foreach (var customer in normalTranses)
            {
                if (!customer.MatchId.Equals(oldMatchId) || messageId == 0)
                {
                    // Create message
                    messageId = CreateMessage(message, customer.MatchId);
                }

                var custId = customer.TargetId;
                var custUserName = customer.TargetName;

                var transIdList = normalTranses.Where(c => c.TargetId == custId)
                                               .Select(c => c.TransId)
                                               .OrderBy(t => t)
                                               .ToArray();
                var transIdText = string.Join(", ", transIdList);
                oldMatchId = customer.MatchId;

                var criteria = new CreatePrivateMessageCriteria
                {
                    BetTransId = transIdText,
                    MessageId = messageId,
                    TargetId = custId,
                    TargetName = custUserName,
                    IsAgent = customer.IsAgent
                };

                _repository.CreatePrivateMessage(criteria);
            }
        }

        /// <summary>
        /// Send private message to notify the credit users: customer, agent, master, super
        /// Firstly, create message
        /// Finally, send private message to users.
        /// </summary>
        /// <param name="matchId">Match Id value.</param>
        /// <param name="message">Message object.</param>
        /// <param name="normalTranses">All transaction have been cancelled of credit sites.</param>
        public void SendPrivateMessage(int matchId, Message message, IList<CreditDeletedBet> normalTranses)
        {
            var messageId = 0;
            foreach (var customer in normalTranses)
            {
                if (messageId == 0)
                {
                    // Create message
                    messageId = CreateMessage(message, matchId);
                }

                var custId = customer.TargetId;
                var custUserName = customer.TargetName;

                var transIdList = normalTranses.Where(c => c.TargetId == custId)
                                               .Select(c => c.TransId)
                                               .OrderBy(t => t)
                                               .ToArray();
                var transIdText = string.Join(", ", transIdList);

                var criteria = new CreatePrivateMessageCriteria
                {
                    BetTransId = transIdText,
                    MessageId = messageId,
                    TargetId = custId,
                    TargetName = custUserName,
                    IsAgent = customer.IsAgent
                };

                _repository.CreatePrivateMessage(criteria);
            }
        }

        /// <summary>
        /// Upload Batch Message.
        /// </summary>
        /// <param name="message">Message value.</param>
        /// <param name="normalTranses">List trans.</param>
        public void BatchMessageSendPrivateMessage(Message message, IList<CreditDeletedBet> normalTranses)
        {
            if (normalTranses.Count > 0)
            {
                var list = new Hashtable();
                foreach (var item in normalTranses)
                {
                    var custId = item.TargetId;
                    var isAgent = item.IsAgent;
                    var key = string.Join("^", custId, isAgent);
                    var tranId = item.TransId.ToString();

                    if (list[key] != null)
                    {
                        if (list[key].ToString().IndexOf(tranId) == -1)
                        {
                            string oldValue = list[key].ToString();
                            list.Remove(key);
                            list.Add(key, string.Join(",", oldValue, tranId));
                        }
                    }
                    else
                    {
                        list.Add(key, tranId);
                    }
                }

                // Save message to users for cancel their transactions.
                this.SetMessage(list, message, normalTranses);
            }
        }

        #region Message

        /// <summary>
        /// Save message to users for cancel their transactions.
        /// </summary>
        /// <param name="list">List ticket number.</param>
        /// <param name="message">Message value.</param>
        /// <param name="normalTranses">All list trans.</param>
        private void SetMessage(Hashtable list, Message message, IList<CreditDeletedBet> normalTranses)
        {
            var listBet = new List<CreatePrivateMessageCriteria>();
            var messageId = 0;
            foreach (DictionaryEntry key in list)
            {
                // This is a CustID of ticket that is deleted [TargetID Field]
                int custId = Convert.ToInt32(key.Key.ToString().Split('^')[0]);

                // This is a UserName of ticket that is deleted [TargetName Field]
                bool isAgent = Convert.ToBoolean(key.Key.ToString().Split('^')[1]);

                string userName = normalTranses.Where(i => i.TargetId == custId && i.IsAgent == isAgent).Select(
                                                    i => i.TargetName).FirstOrDefault();

                var arrTransids = key.Value.ToString().Split(',');
                Array.Sort(arrTransids);
                var transIds = string.Join(", ", arrTransids);

                if (messageId == 0)
                {
                    // Create message
                    messageId = CreateMessage(message, 0);
                }

                var criteria = new CreatePrivateMessageCriteria
                {
                    BetTransId = transIds,
                    MessageId = messageId,
                    TargetId = custId,
                    TargetName = userName,
                    IsAgent = isAgent
                };

                _repository.CreatePrivateMessage(criteria);
            }
        }

        /// <summary>
        /// Create or update a message.
        /// </summary>
        /// <param name="message">Message object.</param>
        /// <param name="matchId">Match Id value.</param>
        /// <returns>Message Id.</returns>
        private int CreateMessage(Message message, int matchId)
        {
            var criteria = new CreateMessageCriteria
            {
                Creator = message.CreatorId,
                CreatorName = message.CreatorName,
                MatchId = matchId,
                Content_en = CorrectSpace(message.Content_en),
                Content_ch = CorrectSpace(message.Content_ch),
                Content_cs = CorrectSpace(message.Content_cs),
                Content_it = CorrectSpace(message.Content_it),
                Content_jp = CorrectSpace(message.Content_jp),
                Content_th = CorrectSpace(message.Content_th),
                Content_ko = CorrectSpace(message.Content_ko),
                Content_es = CorrectSpace(message.Content_es)
            };

            return _repository.CreateMessage(criteria);
        }

        /// <summary>
        /// Replace @@ in message.
        /// </summary>
        /// <param name="text">Message parameter.</param>
        /// <returns>String result after replace.</returns>
        private string CorrectSpace(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            const string Pattern = "\\s*@@\\s*";
            const string Replacement = " @@ ";

            var regex = new Regex(Pattern);
            var result = regex.Replace(text, Replacement);
            return string.IsNullOrEmpty(result) ? string.Empty : result.Trim();
        }

        #endregion Message
    }
}