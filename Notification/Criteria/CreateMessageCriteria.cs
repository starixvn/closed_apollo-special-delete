// ----------------------------------------------------------------------
// <copyright file="CreateMessageCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>

namespace Fanex.TicketControl.Notification.Criteria
{
    using NAP.Framework.Data;

    /// <summary>
    /// CreateMessage Criteria.
    /// </summary>
    public class CreateMessageCriteria : CriteriaBase
    {
        public string Content_ch { get; set; }

        public string Content_cs { get; set; }

        public string Content_en { get; set; }

        public string Content_es { get; set; }

        public string Content_it { get; set; }

        public string Content_jp { get; set; }

        public string Content_ko { get; set; }

        public string Content_th { get; set; }

        public int Creator { get; set; }

        public string CreatorName { get; set; }

        public bool IsBatch
        {
            get { return true; }
        }

        public bool IsSticky
        {
            get { return false; }
        }

        public bool IsTest
        {
            get { return false; }
        }

        public int MatchId { get; set; }

        public int Msgtype
        {
            get { return 1; }
        }

        public override string GetSettingKey()
        {
            return "Core_Dot_Msg_SetMessage";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}