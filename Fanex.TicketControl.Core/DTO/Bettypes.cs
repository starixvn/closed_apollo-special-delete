// ----------------------------------------------------------------------
// <copyright file="Bettypes.cs" company="Nexcel Solutions Vietnam">
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
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class Bettypes : BaseModel
    {
        public bool LiveIndicator
        {
            get;
            set;
        }

        public int Type
        {
            get;
            set;
        }
    }
}