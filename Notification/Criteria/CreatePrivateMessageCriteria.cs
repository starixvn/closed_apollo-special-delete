// ----------------------------------------------------------------------
// <copyright file="CreatePrivateMessageCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/10/17">Created</change>
// </history>

namespace Fanex.TicketControl.Notification.Criteria
{
    using NAP.Framework.Data;

    /// <summary>
    /// Create Private Message Criteria.
    /// </summary>
    public class CreatePrivateMessageCriteria : CriteriaBase
    {
        public bool IsAgent
        {
            get;
            set;
        }

        public bool IsRead
        {
            get { return false; }
        }

        public int MessageId { get; set; }

        public int TargetId { get; set; }

        public string TargetName { get; set; }

        public string BetTransId { get; set; }

        public override string GetSettingKey()
        {
            return "Core_Dot_Msg_SetUsrMessage";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}