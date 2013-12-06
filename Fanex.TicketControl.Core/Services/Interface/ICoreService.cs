// ----------------------------------------------------------------------
// <copyright file="ICoreService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Services.Interface
{
    using System.Collections.Generic;

    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICoreService
    {
        Result CheckMatchIdWithRefno(CheckMatchIdWithRefNoCriteria criteria);

        IList<Licensee> GetAllLicensees();

        IList<Bettypes> GetBettypes(GetBettypeCriteria criteria);

        IList<Currency> GetCurrency();

        Match GetMatchInfo(GetMatchInfoCriteria criteria);
    }
}