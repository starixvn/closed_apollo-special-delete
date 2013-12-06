// <copyright file="VoidByBetIdController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// <history>
//   <change who="Kaden.Trinh" date="2013.09.26">Implement function void ticket by betid.</change>
// </history>

namespace Fanex.TicketControl.SportBook.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Resources;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Web;
    using Fanex.TicketControl.Core.Web.Controllers;
    using Fanex.TicketControl.Notification.DTO;
    using Fanex.TicketControl.Notification.Service.Impl;
    using Fanex.TicketControl.Notification.Service.Interface;
    using Fanex.TicketControl.Notification.ViewModels;
    using Fanex.TicketControl.SportBook.Criteria;
    using Fanex.TicketControl.SportBook.Services.Implement;
    using Fanex.TicketControl.SportBook.Services.Interface;

    /// <summary>
    /// Void By BetId Controller.
    /// </summary>
    public class VoidByBetIdController : BaseController
    {
        /// <summary>
        /// Contain business for void single ticket.
        /// </summary>
        private readonly IVoidByBetIdService _voidByBetIdService;
        private readonly IMessageService _messageService;
        private readonly BalanceUpService _balanceUpService;
        private readonly IMailService<BetTrans> _mailService;

        /// <summary>
        /// Constructor Void Single Bet
        /// Initializes a new instance of the <see cref="VoidByBetIdController" /> class.
        /// </summary>
        public VoidByBetIdController()
        {
            this._voidByBetIdService = new VoidByBetIdService();
            this._messageService = new MessageService();
            this._balanceUpService = new BalanceUpService();
            this._mailService = new MailService();
        }

        private static bool HasHostPermission
        {
            get
            {
                return PermissionService.HasHostPermission(SpecialDeleteFunctionKeys.VoidByBetId)
                        || PermissionService.HasHostPermission(SpecialDeleteFunctionKeys.VoidByBetIdAll);
            }
        }

        /// <summary>
        /// Load default view of void bet id.
        /// </summary>
        /// <returns>Return ActionResult.</returns>
        public ActionResult Index()
        {
            if (HasHostPermission)
            {
                ViewBag.HasVoidBetIdAll =
                    PermissionService.HasHostPermission(SpecialDeleteFunctionKeys.VoidByBetIdAll)
                        ? true
                        : false;
                ViewBag.MinDate = _balanceUpService.MoveBetsDay().AddDays(1).ToString(FormatKey.ShortDateFormatWithSlash);
                ViewBag.MaxDate = DateTime.Now.ToString(FormatKey.ShortDateFormatWithSlash);
                return this.View();
            }
            else
            {
                return Redirect(NapSettings.Settings.UnauthorizeUrl);
            }
        }

        /// <summary>
        /// Get ticket by bet id.
        /// </summary>
        /// <param name="criteria">Criteria for getting single ticket.</param>
        /// <returns>Return tickets.</returns>
        [HttpPost]
        public JsonResult GetTicket(GetVoidByBetIdCriteria criteria)
        {
            criteria.IsShowAll =
                   PermissionService.HasHostPermission(SpecialDeleteFunctionKeys.VoidByBetIdAll)
                       ? true
                       : false;
            return this.Json(this._voidByBetIdService.GetTicket(criteria));
        }

        /// <summary>
        /// Execute void by betId ticket.
        /// </summary>
        /// <param name="criteria">Criteria for executing ticket.</param>
        /// <returns>Return the result of executing ticket.</returns>
        [HttpPost]
        public JsonResult VoidTicket(VoidByBetIdCriteria criteria)
        {
            if (HasHostPermission)
            {
                criteria.UserPwd = Fanex.Common.Cryptography.CfsEnCode(criteria.UserPwd);
                criteria.UserID = CurrentSession.UserId;
                criteria.Message.CreatorId = CurrentSession.UserId;
                criteria.Message.CreatorName = CurrentSession.Username;
                string[] functionName = { SpecialDeleteFunctionKeys.VoidSportsbookBet };
                if (PermissionService.HasHostPermission(SpecialDeleteFunctionKeys.VoidByBetIdAll))
                {
                    criteria.IsDeleteAll = true;
                }
                else
                {
                    criteria.IsDeleteAll = false;
                }

                VoidBetResult<BetTrans> voidBetInformationResult;
                var voidBetResult = _voidByBetIdService.VoidTicket(criteria, out voidBetInformationResult);
                if (voidBetResult.ErrorCode == 0)
                {
                    try
                    {
                        _messageService.SendPrivateMessage(criteria.Message, voidBetInformationResult.ListMessage);
                        voidBetResult.ErrorMsg = Messages.VoidBetSuccessfully;
                    }
                    catch (Exception)
                    {
                        voidBetResult.ErrorCode = 1;
                        voidBetResult.ErrorMsg = Messages.InformMessageToInternalTeam;
                    }

                    var voidBetEmailViewModel = this.NewValueVoidBetEmail(functionName);

                    // Send email to CSD or IT group with UUS and real ticket.
                    _mailService.SendEmailToGroup(voidBetInformationResult.AllTrans, voidBetInformationResult.UserNegativeBalance, voidBetEmailViewModel);
                    voidBetResult.ErrorMsg = Messages.VoidBetSuccessfully;
                }

                return Json(voidBetResult);
            }
            else
            {
                return this.Json(new Core.DTO.Result { ErrorCode = 403, ErrorMsg = Messages.NotAuthorized });
            }
        }

        /// <summary>
        /// Create new value to void bet email.
        /// </summary>
        /// <param name="functionName">Function Name.</param>
        /// <returns>New value.</returns>
        private VoidBetEmailViewModel NewValueVoidBetEmail(string[] functionName)
        {
            return new VoidBetEmailViewModel
             {
                 ActorUsername = CurrentSession.Username,
                 IsITGroup = CurrentSession.GroupName.Equals(Constant.IT),
                 Title = string.Format(Messages.VoidEmailTitle, DateTime.Now.ToString(FormatKey.LongDateFormatWithSlash)),
                 FunctionName = functionName,
                 EmailTemplate = EmailTemplate.VoidByBetID,
                 ShowFullChoice = true
             };
        }
    }
}