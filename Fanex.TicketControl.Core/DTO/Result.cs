// ----------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">Created</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.DTO
{
    using System;

    /// <summary>
    /// Result class.
    /// </summary>
    [Serializable]
    public class Result
    {
        public Result()
        {
            ErrorCode = 0;
            ErrorMsg = string.Empty;
        }

        public int ErrorCode
        {
            get;
            set;
        }

        public string ErrorMsg
        {
            get;
            set;
        }
    }
}