// ----------------------------------------------------------------------
// <copyright file="IBalanceUpService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/09/27">Implement function get special date.</change>
// </history>

namespace Fanex.TicketControl.Core.Services.Interface
{
    using System;

    internal interface IBalanceUpService
    {
        DateTime GetBalanceUpDay();

        DateTime MoveBetsDay();
    }
}