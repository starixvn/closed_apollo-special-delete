// --------------------------------------------------------------------------
// <copyright file="BatchMessageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.Misc.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Web;
    using Fanex.TicketControl.Core.Web.Controllers;
    using Fanex.TicketControl.Misc.Services.Impl;
    using Fanex.TicketControl.Misc.Services.Interface;

    /// <summary>
    ///  Batch Message.
    /// </summary>
    public class BatchMessageController : BaseController
    {
        private readonly IBatchMessageService _service;

        public BatchMessageController()
        {
            _service = new BatchMessageService();
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.UploadBatchMessage, Policy = Constant.Policy.Host)]
        public System.Web.Mvc.ActionResult Index()
        {
            return View();
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.UploadBatchMessage, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult UpdateMessage(Message message, string transIds)
        {
            message.CreatorId = CurrentSession.UserId;
            message.CreatorName = CurrentSession.Username;

            return Json(_service.UpdateMessage(message, transIds));
        }
    }
}