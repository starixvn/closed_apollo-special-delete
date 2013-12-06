// ----------------------------------------------------------------------
// <copyright file="CommonController.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.Controllers
{
    using System.Web.Mvc;

    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommonController : BaseController
    {
        private ICoreService _coreService;

        public CommonController()
        {
            _coreService = new CoreService();
        }

        [HttpPost]
        public JsonResult GetBettypes(GetBettypeCriteria criteria)
        {
            return Json(_coreService.GetBettypes(criteria));
        }

        [HttpPost]
        public JsonResult GetLicensees()
        {
            return Json(_coreService.GetAllLicensees());
        }
    }
}