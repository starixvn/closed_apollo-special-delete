// ----------------------------------------------------------------------
// <copyright file="MailService.cs" company="Nexcel Solutions Vietnam">
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
    public class MailService : BaseMailService, IMailService<BetTrans>
    {
        /// <summary>
        /// Send mail to Group User.
        /// </summary>
        /// <param name="allTickets">All list tickets.</param>
        /// <param name="users">List users negative balance.</param>
        /// <param name="viewModel">Email View model.</param>
        public void SendEmailToGroup(IList<BetTrans> allTickets, IList<NegativeBalance> users, VoidBetEmailViewModel viewModel)
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
        public void SendEmailToCSD(IList<BetTrans> realTickets, string userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel)
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
        public void SendEmailToIT(IList<BetTrans> uusTickets, string userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel)
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
        /// <param name="voidBetEmailViewModel">Void Bet Email View Model.</param>
        /// <returns>Body email value.</returns>
        private string GetSpecialDeleteEmailBody(IEnumerable<BetTrans> tickets, string users, VoidBetEmailViewModel voidBetEmailViewModel)
        {
            var index = 1;

            // For void by BetID. Because it don't need match info.
            var homeId = 0;
            var awayId = 0;

            if (voidBetEmailViewModel.Match != null)
            {
                homeId = voidBetEmailViewModel.Match.HomeId;
                awayId = voidBetEmailViewModel.Match.AwayId;
                voidBetEmailViewModel.Match.EventDate =
                    Convert.ToDateTime(voidBetEmailViewModel.Match.EventDate).ToString(FormatKey.ShortDateFormatWithSlash);
            }

            voidBetEmailViewModel.UserNegativeBalance = users;
            voidBetEmailViewModel.Transactions = (from item in tickets
                                                  let ticketDetail = GetTicketDetail(item, homeId, awayId, voidBetEmailViewModel.ShowFullChoice)
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

            return GetEmailBody(voidBetEmailViewModel.EmailTemplate, voidBetEmailViewModel);
        }

        /// <summary>
        /// Get Choice ticket.
        /// </summary>
        /// <param name="item">Deleted Bet ticket.</param>
        /// <param name="homeId">Home ID input.</param>
        /// <param name="awayId">Away ID input.</param>
        /// <param name="showFullChoice">Show full choice.</param>
        /// <returns>Choice ticket info.</returns>
        private TicketDetail GetTicketDetail(BetTrans item, long homeId, long awayId, bool showFullChoice)
        {
            var tickeDetail = new TicketDetail();
            var ticketHelper = new TicketHelper();
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            var betTypeId = item.BetType;
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            tickeDetail.Ticket = new Ticket
            {
                BetId = item.TransId,
                BetTypeId = betTypeId,
                BetTeam = item.BetTeam,
                LiveHomeScore = (int)item.LiveHomeScore,
                LiveAwayScore = (int)item.LiveAwayScore,
                Stake = item.Stake,
                Odds = item.Odds,
                OddsType = item.OddsType.ToString(CultureInfo.InvariantCulture),
                Handicap1 = item.Hdp1,
                Handicap2 = item.Hdp2,
                Status = item.Status,
                HomeId = homeId == 0 ? item.HomeId : homeId,
                AwayId = awayId == 0 ? item.AwayId : awayId,
                LeagueId = item.LeagueId,
                SportTypeId = item.SportType
            };

            if (!showFullChoice)
            {
                ticketHelper.ShowBetType = false;
                ticketHelper.ShowLeague = false;
                ticketHelper.ShowMatch = false;
            }

            tickeDetail.ChoiceHtml = builder.BuildChoice(tickeDetail.Ticket, ticketHelper, null, false);

            var oddsHtml = builder.BuildOdds(tickeDetail.Ticket, null, funcGetOddsTypeName);
            var oddName = OddsText.GetOddsName(oddsHtml);
            oddName = string.IsNullOrWhiteSpace(oddName)
                ? oddsHtml.Replace("<br />", " ")
                : oddsHtml.Replace("<br />", " (") + ")";
            tickeDetail.OddHtml = oddName;

            if (betTypeId == BetTypeConstant.MixParlay || betTypeId == BetTypeConstant.SystemParlay)
            {
                ticketHelper.IsShowOddsWithChoice = true;
            }

            return tickeDetail;
        }

        #endregion Send mail Helper
    }
}