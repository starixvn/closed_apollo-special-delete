//----------------------------------------------------------------------------------------------
// <copyright file="ITicketListingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam.  All rights reserved.
// </copyright>
//-------------------------------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Repositories.Interface
{
    using System.Data;
    using Fanex.TicketControl.Misc.Criteria;
    using Fanex.TicketControl.Misc.ViewModels;

    public interface ITicketListingRepository
    {
        TicketListingModel GetTicketListing(GetTicketsListingCriteria criteria);

        DataSet GetTicketListingBetList(GetTicketsListingDetailCriteria criteria);
    }
}