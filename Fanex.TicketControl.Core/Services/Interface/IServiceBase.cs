// --------------------------------------------------------------------------
// <copyright file="IServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.9">Created</change>
//   <change who="Ricky.Nguyen" date="2012.11.6">Move</change>
// </history>
namespace Fanex.TicketControl.Core.Services.Interface
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Fanex.NAP.Framework.Data;

    public interface IServiceBase
    {
        /// <summary>
        /// Get IEnumerable.
        /// </summary>
        /// <typeparam name="T">Type input.</typeparam>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Return list.</returns>
        IEnumerable<T> GetListData<T>(ICriteria criteria);
    }
}