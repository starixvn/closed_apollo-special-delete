// --------------------------------------------------------------------------
// <copyright file="VoidEarlyKOBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header to check styleCop</change>
// </history>
namespace Fanex.TicketControl.SportBook.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Resources;
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
    /// Void early.
    /// </summary>
    public class VoidEarlyKOBetController : BaseController
    {
        private readonly IVoidEarlyKOBetService _voidEarlyKOBetService;
        private readonly IMessageService _messageService;
        private readonly IMailService<BetTransEarlyKickOff> _mailService;

        /// <summary>
        /// Constructor Void Early Kick-off Bet
        /// Initializes a new instance of the <see cref="VoidEarlyKOBetController" /> class.
        /// </summary>
        public VoidEarlyKOBetController()
        {
            _voidEarlyKOBetService = new VoidEarlyKOBetService();
            this._messageService = new MessageService();
            this._mailService = new MailEarlyKickOffService();
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidEarlyKOBet, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View();
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidEarlyKOBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult GetEarlyKOTicket(GetEarlyKOBetCriteria criteria)
        {
            return Json(_voidEarlyKOBetService.GetEarlyKOTicket(criteria));
        }

        /// <summary>
        /// Void early kick-off.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidEarlyKOBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult VoidEarlyKOBet(VoidEarlyKOBetCritetia criteria)
        {
            criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);
            criteria.UserId = CurrentSession.UserId;
            criteria.Message.CreatorId = CurrentSession.UserId;
            criteria.Message.CreatorName = CurrentSession.Username;

            VoidBetResult<BetTransEarlyKickOff> voidBetInformationResult;
            var voidBetResult = _voidEarlyKOBetService.VoidEarlyKOBet(criteria, out voidBetInformationResult);

            if (voidBetResult.ErrorCode == 0)
            {
                // Send private message to notify the credit users: customer, agent
                try
                {
                    _messageService.SendPrivateMessage(criteria.MatchId, criteria.Message, voidBetInformationResult.ListMessage);
                }
                catch (Exception)
                {
                    voidBetResult.ErrorCode = 1;
                    voidBetResult.ErrorMsg = Messages.InformMessageToInternalTeam;
                }

                var matchInfo = voidBetInformationResult.Match;
                var voidBetEmailViewModel = new VoidBetEmailViewModel
                {
                    Match = matchInfo,
                    ActorUsername = CurrentSession.Username,
                    IsITGroup = CurrentSession.GroupName.Equals(Constant.IT),
                    Title = string.Format(Messages.VoidEarlyKickOffEmailTitle, matchInfo.DateTime),
                    FunctionName = new string[] { SpecialDeleteFunctionKeys.VoidSportsbookBet },
                    EmailTemplate = EmailTemplate.VoidEarlyKickOff
                };

                // Send email to CSD or IT group with UUS and real ticket.
                _mailService.SendEmailToGroup(voidBetInformationResult.AllTrans, voidBetInformationResult.UserNegativeBalance, voidBetEmailViewModel);
                voidBetResult.ErrorMsg = Messages.VoidBetSuccessfully;
            }

            return Json(voidBetResult);
        }
    }
}