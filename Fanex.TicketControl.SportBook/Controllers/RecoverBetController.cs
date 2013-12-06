// --------------------------------------------------------------------------
// <copyright file="RecoverBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Resources;
    using Fanex.TicketControl.Core.Utils;
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
    /// Recover controller.
    /// </summary>
    public class RecoverBetController : BaseController
    {
        private readonly IRecoverBetService _serviceRecover;
        private readonly IMessageService _messageService;
        private readonly IMailService<BetTrans> _mailService;

        /// <summary>
        /// Constructor Recover Bet
        /// Initializes a new instance of the <see cref="RecoverBetController" /> class.
        /// </summary>
        public RecoverBetController()
        {
            this._serviceRecover = new RecoverBetService();
            this._messageService = new MessageService();
            this._mailService = new MailService();
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.RecoverSportsbookBet, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            return View();
        }

        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.RecoverSportsbookBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult GetTicketRecover(GetRecoverBetCriteria criteria)
        {
            var rs = new Result();

            var data = _serviceRecover.GetTicketRecover(criteria);
            return data.Result.ErrorCode == 0 ? Json(data) : Json(rs);
        }

        /// <summary>
        /// Submit recover ticket.
        /// </summary>
        /// <param name="criteria">Recover Bet Criteria.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SpecialDeleteFunctionKeys.RecoverSportsbookBet, Policy = Constant.Policy.Host)]
        [HttpPost]
        public JsonResult RecoverBet(RecoverBetCriteria criteria)
        {
            criteria.UserId = CurrentSession.UserId;
            criteria.Ip = Helper.GetClientIp();
            criteria.Message.CreatorId = CurrentSession.UserId;
            criteria.Message.CreatorName = CurrentSession.Username;

            VoidBetResult<BetTrans> voidBetInformationResult;
            var recoverResult = _serviceRecover.RecoverBet(criteria, out voidBetInformationResult);

            if (recoverResult.ErrorCode == 0)
            {
                // Send private message to notify the credit users: customer, agent
                try
                {
                    _messageService.SendPrivateMessage(criteria.Message, voidBetInformationResult.ListMessage);
                }
                catch (Exception)
                {
                    recoverResult.ErrorCode = 1;
                    recoverResult.ErrorMsg = Messages.InformMessageToInternalTeam;
                }

                var matchInfo = voidBetInformationResult.Match;
                var voidBetEmailViewModel = new VoidBetEmailViewModel
                {
                    Match = matchInfo,
                    ActorUsername = CurrentSession.Username,
                    IsITGroup = CurrentSession.GroupName.Equals(Constant.IT),
                    Title = string.Format(Messages.RecoverEmailTitle, matchInfo.DateTime),
                    FunctionName = new string[] { SpecialDeleteFunctionKeys.RecoverSportsbookBet },
                    EmailTemplate = EmailTemplate.RecoverBet
                };

                // Send email to CSD or IT group with UUS and real ticket.
                _mailService.SendEmailToGroup(voidBetInformationResult.AllTrans, voidBetInformationResult.UserNegativeBalance, voidBetEmailViewModel);

                if (string.IsNullOrEmpty(recoverResult.ErrorMsg))
                {
                    recoverResult.ErrorMsg = string.Format(Messages.RecoverBet, criteria.TransId);
                }
                else
                {
                    recoverResult.ErrorCode = -1; // We need show warning when the match is settled.
                }
            }

            return Json(recoverResult);
        }
    }
}