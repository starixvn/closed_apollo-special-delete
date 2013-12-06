// <copyright file="VoidSingleBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
//   <change who="Kaden.Trinh" date="2013.09.19">Implement function void ticket is settled.</change>
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
    using Fanex.TicketControl.Core.Criteria;
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
    /// Void Single Ticket Controller.
    /// </summary>
    public class VoidSingleBetController : BaseController
    {
        /// <summary>
        /// Contain business for void single ticket.
        /// </summary>
        private readonly IVoidSingleBetService _voidSingleBetService;
        private readonly IMessageService _messageService;
        private readonly IMailService<BetTrans> _mailService;

        /// <summary>
        /// Constructor Void Single Ticket
        /// Initializes a new instance of the <see cref="VoidSingleBetController" /> class.
        /// </summary>
        public VoidSingleBetController()
        {
            this._voidSingleBetService = new VoidSingleBetService();
            this._messageService = new MessageService();
            this._mailService = new MailService();
        }

        /// <summary>
        /// Load default view of void single ticket.
        /// </summary>
        /// <returns>Return ActionResult.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidSingleBet, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Get Single ticket.
        /// </summary>
        /// <param name="criteria">Criteria for getting single ticket.</param>
        /// <returns>Return tickets.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidSingleBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult GetSingleBet(GetSingleBetCriteria criteria)
        {
            criteria.UserName = CurrentSession.Username;
            return this.Json(this._voidSingleBetService.GetSingleBet(criteria));
        }

        /// <summary>
        /// Check valid input parameter of a match.
        /// </summary>
        /// <param name="criteria">Criteria for checking input parameter of a match.</param>
        /// <returns>Result of checking parameter.</returns>
        [HttpPost]
        public JsonResult CheckMatchIdWithRefno(CheckMatchIdWithRefNoCriteria criteria)
        {
            criteria.Type = SportType.Sportsbook;
            return this.Json(this._voidSingleBetService.CheckMatchIDwithRefno(criteria));
        }

        /// <summary>
        /// Execute void single ticket.
        /// </summary>
        /// <param name="criteria">Criteria for executing void single ticket.</param>
        /// <returns>Return the result of executing void single ticket.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.VoidSingleBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult VoidBet(VoidSingleBetCritetia criteria)
        {
            criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);
            criteria.UserId = CurrentSession.UserId;
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
                    Title = string.Format(Messages.VoidEmailTitle, matchInfo.DateTime),
                    FunctionName = new string[] { SpecialDeleteFunctionKeys.VoidSportsbookBet },
                    EmailTemplate = EmailTemplate.VoidSingleBet
                };

                // Send email to CSD or IT group with UUS and real ticket.
                _mailService.SendEmailToGroup(voidBetInformationResult.AllTrans, voidBetInformationResult.UserNegativeBalance, voidBetEmailViewModel);
                voidBetResult.ErrorMsg = Messages.VoidBetSuccessfully;
            }

            return Json(voidBetResult);
        }
    }
}