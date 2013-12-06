// ----------------------------------------------------------------------
// <copyright file="CheckMatchIdWithRefNoCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Criteria
{
    using Fanex.NAP.Framework.Data;
    using Fanex.TicketControl.Core.Constants;

    /// <summary>
    /// Criteria checking match.
    /// </summary>
    public class CheckMatchIdWithRefNoCriteria : CriteriaBase
    {
        public int ErrCode
        {
            get;
            set;
        }

        public string ErrMsg
        {
            get;
            set;
        }

        public long FromRefno
        {
            get;
            set;
        }

        public int MatchId
        {
            get;
            set;
        }

        public long ToRefno
        {
            get;
            set;
        }

        public SportType Type
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "Core_CheckMatchIDwithRefno";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}