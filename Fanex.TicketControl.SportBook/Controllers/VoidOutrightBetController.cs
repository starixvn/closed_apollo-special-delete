// --------------------------------------------------------------------------
// <copyright file="VoidOutrightBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.10.20">Created</change>
//   <change who="Kaden.Trinh" date="2013.10.23">Implement function send mail.</change>
// </history>

namespace Fanex.TicketControl.SportBook.Controllers
{
    using System;
    using System.Collections.Generic;
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
    /// Void outright ticket.
    /// </summary>
    public class VoidOutrightBetController : BaseController
    {
        private readonly IVoidOutrightBetService _voidOutrightBetService;
        private readonly IVoidSingleBetService _voidSingleBetService;
        private readonly IMessageService _messageService;
        private readonly IMailService<BetTrans> _mailService;

        public VoidOutrightBetController()
        {
            this._messageService = new MessageService();
            this._mailService = new MailService();
            _voidOutrightBetService = new VoidOutrightBetService();
            _voidSingleBetService = new VoidSingleBetService();
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidOutrightBet, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetOutrightLeague()
        {
            return Json(_voidOutrightBetService.GetOutrightLeague());
        }

        [HttpPost]
        public JsonResult GetTeamByLeague(GetOutrightTeamCriteria criteria)
        {
            return Json(_voidOutrightBetService.GetTeamByLeague(criteria));
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidOutrightBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult GetOutrightBet(GetOutrightBetCriteria criteria)
        {
            criteria.UserName = CurrentSession.Username;
            return Json(_voidOutrightBetService.GetOutrightBet(criteria));
        }

        /// <summary>
        /// Void outright ticket.
        /// </summary>
        /// <param name="criteria">Criteria for executing.</param>
        /// <returns>Return the result of executing void outright ticket.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidOutrightBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult VoidOutrightBet(VoidSingleBetCritetia criteria)
        {
            criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);
            criteria.UserId = CurrentSession.UserId;
            criteria.Betteam1378 = string.Empty;
            criteria.Betteam515 = string.Empty;
            criteria.HDP = 9999;
            criteria.Message.CreatorId = CurrentSession.UserId;
            criteria.Message.CreatorName = CurrentSession.Username;

            VoidBetResult<BetTrans> voidBetInformationResult;
            var voidBetResult = _voidSingleBetService.VoidSingleBet(criteria, out voidBetInformationResult);

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
                    Title = string.Format(Messages.VoidOutrightEmailTitle, matchInfo.DateTime),
                    FunctionName = new string[] { SpecialDeleteFunctionKeys.VoidSportsbookBet },
                    EmailTemplate = EmailTemplate.VoidOutrightBet
                };

                // Send email to CSD or IT group with UUS and real ticket.
                _mailService.SendEmailToGroup(voidBetInformationResult.AllTrans, voidBetInformationResult.UserNegativeBalance, voidBetEmailViewModel);
                voidBetResult.ErrorMsg = Messages.VoidBetSuccessfully;
            }

            return Json(voidBetResult);
        }
    }
}