// ----------------------------------------------------------------------
// <copyright file="IBatchMessageService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.30">Check StyleCop</change>
// </history>

namespace Fanex.TicketControl.Misc.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.TicketControl.Core.DTO;

    public interface IBatchMessageService
    {
        Result UpdateMessage(Message message, string transIds);
    }
}