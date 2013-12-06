// ----------------------------------------------------------------------
// <copyright file="MailParlayService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/10/23">Implement function send mail to CSD or IT</change>
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
    public class MailParlayService : BaseMailService, IMailService<BetTransParlay>
    {
        /// <summary>
        /// Send mail to Group User.
        /// </summary>
        /// <param name="allTickets">All list tickets.</param>
        /// <param name="users">List users negative balance.</param>
        /// <param name="voidBetEmailViewModel">Email View model.</param>
        public void SendEmailToGroup(IList<BetTransParlay> allTickets, IList<NegativeBalance> users, VoidBetEmailViewModel voidBetEmailViewModel)
        {
            var uusTickets = allTickets.Where(s => s.CurrencyName.Equals(CurrencyConstant.Uus)).ToArray();
            var realTickets = allTickets.Where(s => !s.CurrencyName.Equals(CurrencyConstant.Uus)).ToArray();

            var uusUsers = string.Join(", ", users.Where(s => s.CurrencyName.Equals(CurrencyConstant.Uus)).Select(m => m.Username));
            var realUsers = string.Join(", ", users.Where(s => !s.CurrencyName.Equals(CurrencyConstant.Uus)).Select(m => m.Username));

            // Void/Recover real ticket then send to CSD and BCC IT.
            if (realTickets.Count() > 0)
            {
                this.SendEmailToCSD(realTickets, realUsers, voidBetEmailViewModel);
            }

            // This is login by IT Group
            if (voidBetEmailViewModel.IsITGroup)
            {
                var title = voidBetEmailViewModel.Title;

                // Void/Recover UUS ticket.
                if (uusTickets.Count() > 0)
                {
                    voidBetEmailViewModel.Title = string.Join(" - ", Messages.TestCurrency, title);
                    this.SendEmailToIT(uusTickets, uusUsers, voidBetEmailViewModel);
                }

                // Void/Recover real ticket.
                if (realTickets.Count() > 0)
                {
                    voidBetEmailViewModel.Title = string.Join(" - ", Messages.RealCurrency, title);
                    this.SendEmailToIT(realTickets, realUsers, voidBetEmailViewModel);
                }
            }
            else
            {
                // Void/Recover UUS ticket.
                if (uusTickets.Count() > 0)
                {
                    this.SendEmailToCSD(uusTickets, uusUsers, voidBetEmailViewModel);
                }
            }
        }

        /// <summary>
        /// Real tickets then send Email To CSD.
        /// </summary>
        /// <param name="realTickets">Deleted Bets list.</param>
        /// <param name="userNegativeBalance">Users negative balance.</param>
        /// <param name="voidBetEmailViewModel">Void Bet Email View Model.</param>
        public void SendEmailToCSD(IList<BetTransParlay> realTickets, string userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel)
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
                    var title = voidBetEmailViewModel.Title;

                    // Send email
                    var mailRecord = new MailRecord
                    {
                        MailTo = mailCsd,
                        MailCc = mailCC,
                        MailBcc = mailIt,
                        Title = title,
                        Subject = title,
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
        public void SendEmailToIT(IList<BetTransParlay> uusTickets, string userNegativeBalance, VoidBetEmailViewModel voidBetEmailViewModel)
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
                    var title = voidBetEmailViewModel.Title;

                    // Send email
                    var mailRecord = new MailRecord
                    {
                        MailTo = mailTo,
                        MailCc = mailCC,
                        MailBcc = mailBcc,
                        Title = title,
                        Subject = title,
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
        /// Set new value to ticket.
        /// </summary>
        /// <param name="item">Item value.</param>
        /// <returns>Return ticket.</returns>
        private static Ticket SetValue(BetTransParlay item)
        {
            return new Ticket
            {
                BetId = item.TransId,
                BetTypeId = item.BetType,
                TransId = item.SubTransId,
                RefNo = item.RefNo,
                BetTeam = item.BetTeam,
                LiveHomeScore = item.Choice_LiveHomeScore,
                LiveAwayScore = item.Choice_LiveAwayScore,
                IsLive = item.Choice_IsLiveIndicator,
                Stake = item.Stake,
                Odds = item.Odds,
                OddsType = item.OddsType.ToString(CultureInfo.InvariantCulture),
                MasterOdds = item.MOdds,
                Handicap1 = item.Choice_Hdp1,
                Handicap2 = item.Choice_Hdp2,
                Status = item.Status,
                LeagueId = item.Choice_LeagueId,
                SportTypeId = item.Choice_SportType,
                HomeId = item.Choice_HomeId,
                AwayId = item.Choice_AwayId,
            };
        }

        /// <summary>
        /// Ticket detail.
        /// </summary>
        /// <param name="item">Bet trans data.</param>
        /// <returns>Return ticket detail.</returns>
        private static TicketDetail GetTicketDetail(BetTransParlay item)
        {
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            var betTypeId = item.BetType;

            var tickeDetail = new TicketDetail()
            {
                Ticket = SetValue(item)
            };

            var ticketHelper = new TicketHelper
            {
                ShowSystemParlayDetail = false,
                IsShowOddsWithChoice = true
            };

            var choice = builder.BuildChoice(tickeDetail.Ticket, ticketHelper, null, false);
            var choiceMp = choice;

            var oddsHtml = builder.BuildOdds(tickeDetail.Ticket, null, funcGetOddsTypeName);
            var oddName = OddsText.GetOddsName(oddsHtml);
            oddName = string.IsNullOrWhiteSpace(oddName)
                ? oddsHtml.Replace("<br />", " ")
                : oddsHtml.Replace("<br />", " (") + ")";
            tickeDetail.OddHtml = oddName;

            tickeDetail.Ticket.Odds = item.Choice_Odds;
            tickeDetail.Ticket.BetTeam = item.Choice_BetTeam;
            tickeDetail.Ticket.BetTypeId = item.Choice_BetType;
            tickeDetail.Ticket.HomeId = item.Choice_HomeId;
            tickeDetail.Ticket.AwayId = item.Choice_AwayId;

            var choiceFavorite = builder.BuildChoiceBlock(tickeDetail.Ticket, ticketHelper, null, false);
            choiceFavorite.ScoreMapIcon = null;
            choiceFavorite.BetType = null;
            choiceFavorite.Match = null;
            choiceFavorite.League = null;
            var subTransId = item.SubTransId.ToString();
            choiceMp = choiceMp.Replace("<br/><br/>", string.Empty);
            var strChoice = choiceMp.Replace("<div id=", "<div id='dvFavorite_" + subTransId + "'>" + choiceFavorite + "</div><div id=");
            strChoice += "<div id='divEvent_" + subTransId + "'></div>";
            tickeDetail.ChoiceHtml = strChoice;

            return tickeDetail;
        }

        /// <summary>
        /// Get Email Body.
        /// </summary>
        /// <param name="tickets">Transactions List.</param>
        /// <param name="users">Users negative balance.</param>
        /// <param name="viewModel">Void Bet Email View Model.</param>
        /// <returns>Body email value.</returns>
        private string GetSpecialDeleteEmailBody(IEnumerable<BetTransParlay> tickets, string users, VoidBetEmailViewModel viewModel)
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
                                          Status = item.Status,
                                          Choice = ticketDetail.ChoiceHtml
                                      }).ToList();

            return GetEmailBody(viewModel.EmailTemplate, viewModel);
        }

        #endregion Send mail Helper
    }
}