// --------------------------------------------------------------------------
// <copyright file="VoidMixParlayBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
//   <change who="Kaden.Trinh" date="2013.09.23">Implement function void ticket is settled.</change>
//   <change who="Kaden.Trinh" date="2013.10.23">Implement send mail function.</change>
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
    /// Void mix parlay controller.
    /// </summary>
    public class VoidMixParlayBetController : BaseController
    {
        private readonly IVoidParlayBetService _voidParlayBetService;
        private readonly IMessageService _messageService;
        private readonly IMailService<BetTransParlay> _mailService;

        public VoidMixParlayBetController()
        {
            _voidParlayBetService = new VoidParlayBetService();
            this._messageService = new MessageService();
            this._mailService = new MailParlayService();
        }

        /// <summary>
        /// Load default parlay ticket page.
        /// </summary>
        /// <returns>Return ActionResult.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidParlayBet, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get list parlay ticket.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidParlayBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult GetParlayBet(GetParlayBetCriteria criteria)
        {
            return Json(_voidParlayBetService.GetParlayBet(criteria));
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidParlayBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult GetParlaySubTicket(GetParlaySubTicketCriteria criteria)
        {
            return Json(_voidParlayBetService.GetParlaySubTicket(criteria));
        }

        /// <summary>
        /// Void parlay ticket.
        /// </summary>
        /// <param name="criteria">Criteria ticket.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidParlayBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult VoidParlayBet(VoidParlayBetCritetia criteria)
        {
            criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);
            criteria.UserId = CurrentSession.UserId;
            criteria.Message.CreatorId = CurrentSession.UserId;
            criteria.Message.CreatorName = CurrentSession.Username;

            VoidBetResult<BetTransParlay> voidBetInformationResult;
            var voidParlayResult = _voidParlayBetService.VoidParlayBet(criteria, out voidBetInformationResult);

            if (voidParlayResult.ErrorCode == 0)
            {
                // Send private message to notify the credit users: customer, agent
                try
                {
                    _messageService.SendPrivateMessage(criteria.MatchId, criteria.Message, voidBetInformationResult.ListMessage);
                }
                catch (Exception)
                {
                    voidParlayResult.ErrorCode = 1;
                    voidParlayResult.ErrorMsg = Messages.InformMessageToInternalTeam;
                }

                var matchInfo = voidBetInformationResult.Match;
                var voidBetEmailViewModel = new VoidBetEmailViewModel
                {
                    Match = matchInfo,
                    ActorUsername = CurrentSession.Username,
                    IsITGroup = CurrentSession.GroupName.Equals(Constant.IT),
                    Title = string.Format(Messages.VoidParlayEmailTitle, matchInfo.DateTime),
                    FunctionName = new string[] { SpecialDeleteFunctionKeys.VoidSportsbookBet },
                    EmailTemplate = EmailTemplate.VoidParlayBet
                };

                // Send email to CSD or IT group with UUS and real ticket.
                _mailService.SendEmailToGroup(voidBetInformationResult.AllTrans, voidBetInformationResult.UserNegativeBalance, voidBetEmailViewModel);
                voidParlayResult.ErrorMsg = Messages.VoidBetSuccessfully;
            }

            return Json(voidParlayResult);
        }
    }
}