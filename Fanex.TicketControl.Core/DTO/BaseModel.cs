// --------------------------------------------------------------------------
// <copyright file="BaseModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Core.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class BaseModel
    {
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}