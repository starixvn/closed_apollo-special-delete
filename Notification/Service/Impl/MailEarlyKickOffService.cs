// ----------------------------------------------------------------------
// <copyright file="MailEarlyKickOffService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/10/20">Implement function send mail to CSD or IT</change>
// </history>

namespace Fanex.TicketControl.Notification.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using BetList.Core.Entities;
    using BetList.Core.Factories;
    using BetList.Core.TicketHelper;
    using Core.Utils;
    using Fanex.BetList.Core.Utils;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.TicketControl.Core.Constants;
    using Fanex.TicketControl.Core.DTO;
    using Fanex.TicketControl.Core.Resources;
    using Fanex.TicketControl.Notification.DTO;
    using Interface;
    using Logging;
    using MailDeliver.Model;
    using ViewModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class MailEarlyKickOffService : BaseMailService, IMailService<BetTransEarlyKickOff>
    {
        /// <summary>
        /// Send mail to Group User.
        /// </summary>
        /// <param name="allTickets">All list tickets.</param>
        /// <param name="users">List users negative balance.</param>
        /// <param name="viewModel">Email View model.</param>
        public void SendEmailToGroup(IList<BetTransEarlyKickOff> allTickets, IList<NegativeBalance> users, VoidBetEmailViewModel viewModel)
        {
            var uusTickets = allTickets.Where(s => s.CurrencyName.Equals(CurrencyConstant.Uus)).ToArray();
            var realTickets = allTickets.Where(s => !s.CurrencyName.Equals(CurrencyConstant.Uus)).ToArray();
            var uusUsers = string.Join(", ", users.Where(s => s.CurrencyName.Equals(CurrencyConstant.Uus)).Select(m => m.Username));
            var realUsers = string.Join(", ", users.Where(s => !s.CurrencyName.Equals(CurrencyConstant.Uus)).Select(m => m.Username));

            // Void/Recover real ticket then send to CSD and BCC IT.
            if (realTickets.Count() > 0)
            {
                this.SendEmailToCSD(realTickets, realUsers, viewModel);
            }

            // This is login by IT Group
            if (viewModel.IsITGroup)
            {
                var title = viewModel.Title;

                // Void/Recover UUS ticket.
                if (uusTickets.Count() > 0)
                {
                    viewModel.Title = string.Join(" - ", Messages.TestCurrency, title);
                    this.SendEmailToIT(uusTickets, uusUsers, viewModel);
                }

                // Void/Recover real ticket.
                if (realTickets.Count() > 0)
                {
                    viewModel.Title = string.Join(" - ", Messages.RealCurrency, title);
                    this.SendEmailToIT(realTickets, realUsers, viewModel);
                }
            }
            else
            {
                // Void/Recover UUS ticket.
                if (uusTickets.Count() > 0)
                {
                    this.SendEmailToCSD(uusTickets, uusUsers, viewModel);
                }
            }
        }

        /// <summary>
        /// Real tickets then send Email To CSD.
        /// </summary>
        /// <param name="realTickets">Deleted Bets list.</param>
        /// <param name="userNegativeBalance">Users negative balance.</param>
        /// <param name="voidBetEmailViewModel">Void Bet Email View Model.</param>
        public void SendEmailToCSD(IList<BetTransEarlyKickOff> realTickets, string userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel)
        {
            try
            {
                var mailCC = string.Empty;
                var mailCsd = string.Empty;
                var mailIt = string.Empty;

                // Get mailing list send to Csd group.
                mailCsd = this.GetMailCSD(voidBetEmailViewModel, out mailCC, out mailIt);

                if (realTickets.Count > 0 && mailCsd.Count() > 0)
                {
                    var emailBody = HttpUtility.HtmlDecode(GetSpecialDeleteEmailBody(realTickets, userNegativeBalance, voidBetEmailViewModel));
                    var subject = voidBetEmailViewModel.Title;

                    // Send email
                    var mailRecord = new MailRecord
                    {
                        MailTo = mailCsd,
                        MailCc = mailCC,
                        MailBcc = mailIt,
                        Title = Messages.EmailFrom,
                        Subject = subject,
                        Body = emailBody
                    };

                    using (var mailClient = new MailDeliver.MailClient.API.MailDeliver())
                    {
                        mailClient.SendMail(mailRecord);
                    }
                }
            }
            catch (Exception exception)
            {
                LogService.Error("Error when send email to CSD group", exception);
            }
        }

        /// <summary>
        /// UUS tickets then send Email To IT.
        /// </summary>
        /// <param name="uusTickets">Tickets UUS.</param>
        /// <param name="userNegativeBalance">Users negative balance.</param>
        /// <param name="voidBetEmailViewModel">Void Bet Email View Model.</param>
        public void SendEmailToIT(IList<BetTransEarlyKickOff> uusTickets, string userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel)
        {
            try
            {
                var mailCC = string.Empty;
                var mailBcc = string.Empty;
                var mailTo = string.Empty;

                // Get mailing list send to IT group.
                mailTo = this.GetMailIT(voidBetEmailViewModel, out mailCC, out mailBcc);

                if (uusTickets.Count > 0 && mailTo.Count() > 0)
                {
                    var emailBody = HttpUtility.HtmlDecode(GetSpecialDeleteEmailBody(uusTickets, userNegativeBalance, voidBetEmailViewModel));
                    var subject = voidBetEmailViewModel.Title;

                    // Send email
                    var mailRecord = new MailRecord
                    {
                        MailTo = mailTo,
                        MailCc = mailCC,
                        MailBcc = mailBcc,
                        Title = Messages.EmailFrom,
                        Subject = subject,
                        Body = emailBody
                    };

                    using (var mailClient = new MailDeliver.MailClient.API.MailDeliver())
                    {
                        mailClient.SendMail(mailRecord);
                    }
                }
            }
            catch (Exception exception)
            {
                LogService.Error("Error when send email to IT group", exception);
            }
        }

        #region Send mail Helper

        /// <summary>
        /// Get Email Body.
        /// </summary>
        /// <param name="tickets">Transactions List.</param>
        /// <param name="users">Users negative balance.</param>
        /// <param name="viewModel">Void Bet Email View Model.</param>
        /// <returns>Body email value.</returns>
        private string GetSpecialDeleteEmailBody(IEnumerable<BetTransEarlyKickOff> tickets, string users, VoidBetEmailViewModel viewModel)
        {
            var index = 1;
            viewModel.Match.EventDate = Convert.ToDateTime(viewModel.Match.EventDate).ToString(FormatKey.ShortDateFormatWithSlash);
            viewModel.UserNegativeBalance = users;
            viewModel.Transactions = (from item in tickets
                                      let ticketDetail = GetTicketDetail(item)
                                      select new VoidedBetViewModel
                                      {
                                          No = index++,
                                          BetId = item.TransId,
                                          Username = item.Username,
                                          BetType = BetTypeText.GetBetTypeName(item.BetType),
                                          OddsType = OddsText.GetOddsTypeName(item.BetType, item.OddsType),
                                          Stake = ConvertByBetType.Stake(item.Stake),
                                          Odds = ticketDetail.OddHtml,
                                          CurrencyName = item.CurrencyName,
                                          TransDate = item.TransDate,
                                          LiveHomeScore = item.LiveHomeScore,
                                          LiveAwayScore = item.LiveHomeScore,
                                          Handicap = HandicapText.GetHandicapText(item.BetType, item.BetTeam, item.Hdp1, item.Hdp2),
                                          BetTeam = BetTeamText.GetBetTeam(item.BetTeam, item.BetType),
                                          SecAcceptFlag = item.SecAcceptFlag ? "x" : string.Empty,
                                          WinLost = item.WinLost,
                                          Status = item.Status,
                                          Choice = ticketDetail.ChoiceHtml
                                      }).ToList();

            return GetEmailBody(viewModel.EmailTemplate, viewModel);
        }

        /// <summary>
        /// New ticket detail.
        /// </summary>
        /// <param name="item">Data row parameter.</param>
        /// <returns>Return ticket.</returns>
        private Ticket SetValue(BetTransEarlyKickOff item)
        {
            return new Ticket()
            {
                BetId = item.TransId,
                BetTypeId = item.IsMP ? item.ChoiceBetType : item.BetType,
                TransId = item.SubTransId,
                RefNo = item.RefNo,
                BetTeam = item.BetTeam,
                LiveHomeScore = item.LiveHomeScore,
                LiveAwayScore = item.LiveAwayScore,
                Stake = item.Stake,
                Odds = item.Odds,
                OddsType = item.OddsType.ToString(),
                MasterOdds = item.MOdds,
                Handicap1 = item.Hdp1,
                Handicap2 = item.Hdp2,
                Status = item.Status,
                HomeId = item.HomeId,
                AwayId = item.AwayId,
                LeagueId = item.LeagueId,
                SportTypeId = item.SportType,
            };
        }

        /// <summary>
        /// Get Choice ticket.
        /// </summary>
        /// <param name="item">Deleted Bet ticket.</param>
        /// <returns>Choice ticket info.</returns>
        private TicketDetail GetTicketDetail(BetTransEarlyKickOff item)
        {
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            var ticketDetail = new TicketDetail { Ticket = SetValue(item) };

            var ticketHelper = new TicketHelper
            {
                ShowLeague = false,
                ShowMatch = false,
                ShowBetType = false,
                IsShowOddsWithChoice = true,
                ShowSystemParlayDetail = false
            };

            ticketDetail.Ticket.BetTypeId = 0;
            ticketDetail.StakeHtml = builder.BuildStake(ticketDetail.Ticket);
            ticketDetail.OddHtml = builder.BuildOdds(ticketDetail.Ticket, null, funcGetOddsTypeName);

            ticketDetail.Ticket.BetTypeId = item.BetType;
            var choice = builder.BuildChoice(ticketDetail.Ticket, ticketHelper, null, false);

            ticketDetail.Ticket.BetTeam = item.ChoiceBetTeam;
            ticketDetail.Ticket.BetTypeId = item.IsMP ? item.ChoiceBetType : item.BetType;

            var choiceFavorite = builder.BuildChoice(ticketDetail.Ticket, ticketHelper, null, false);
            var choiceBlock = builder.BuildChoiceBlock(ticketDetail.Ticket, ticketHelper, null, false);
            choiceBlock.League = null;
            choiceBlock.Match = null;
            choiceBlock.BetType = null;
            var subTransId = item.SubTransId.ToString(CultureInfo.InvariantCulture);
            choice = choice.Replace("<br/><br/>", string.Empty);
            var strChoice = choice.Replace("<div id=", "<div id='dvFavorite_" + subTransId + "'>" + choiceFavorite + "</div><div id=");
            strChoice += "<div id='divEvent_" + subTransId + "'></div>";
            ticketDetail.ChoiceHtml = item.IsMP ? strChoice : choiceBlock;

            return ticketDetail;
        }

        #endregion Send mail Helper
    }
}