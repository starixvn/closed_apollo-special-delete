// ----------------------------------------------------------------------
// <copyright file="BaseMailService.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013/10/30">Implement function send mail to CSD or IT</change>
// </history>
namespace Fanex.TicketControl.Notification.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.TicketControl.Notification.ViewModels;
    using RazorEngine;

    /// <summary>
    /// Base mail service.
    /// </summary>
    public class BaseMailService
    {
        private readonly IMailingListService _mailService;

        public BaseMailService()
        {
            _mailService = new MailingListService();
        }

        /// <summary>
        /// Get email address IT or CSD.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Return mail.</returns>
        public string MailTo(GetMailingListByFunctionKeyAndSiteCriteria criteria)
        {
            // List mail
            var listMail = _mailService.GetByFunctionKeyAndSite(criteria);
            return string.Join(";", listMail.Select(m => m.ToList));
        }

        /// <summary>
        /// Get email address IT or CSD.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <param name="mailCC">Output CC mail.</param>
        /// <param name="mailBcc">Output Bcc mail.</param>
        /// <returns>Return mail to.</returns>
        public string MailTo(GetMailingListByFunctionKeyAndSiteCriteria criteria, out string mailCC, out string mailBcc)
        {
            // List mail
            var listMail = _mailService.GetByFunctionKeyAndSite(criteria);
            mailCC = string.Join(";", listMail.Select(m => m.CCList));
            mailBcc = string.Join(";", listMail.Select(m => m.BCCList));
            return string.Join(";", listMail.Select(m => m.ToList));
        }

        /// <summary>
        /// Get new mail.
        /// </summary>
        /// <param name="oldValue">Old mail value.</param>
        /// <param name="newValue">New mail value.</param>
        /// <returns>Return new value.</returns>
        public string GetNewValue(string oldValue, string newValue)
        {
            return string.IsNullOrEmpty(newValue) ? oldValue : string.Join(";", newValue, oldValue);
        }

        /// <summary>
        /// Get the whole content of a email template file.
        /// T: Anonymous type.
        /// </summary>
        /// <typeparam name="T">View model for the template.</typeparam>
        /// <param name="templatePath">Path to the template file.</param>
        /// <param name="model">Data for mapping into email template.</param>
        /// <returns>Email body.</returns>
        public string GetEmailBody<T>(string templatePath, T model)
        {
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatePath);
            var template = File.ReadAllText(fullPath);

            Razor.Compile(template, typeof(T), templatePath);
            return Razor.Run(templatePath, model);
        }

        /// <summary>
        /// Get mailing list for IT.
        /// </summary>
        /// <param name="voidBetEmailViewModel">View model value.</param>
        /// <param name="outMailCC">Out CC mail address value.</param>
        /// <param name="outMailBcc">Out Bcc mail address value.</param>
        /// <returns>Return list mail To.</returns>
        public string GetMailIT(VoidBetEmailViewModel voidBetEmailViewModel, out string outMailCC, out string outMailBcc)
        {
            var mailCC = string.Empty;
            var mailBcc = string.Empty;
            var mailTo = string.Empty;

            // Loop over function keys.
            foreach (var functionKey in voidBetEmailViewModel.FunctionName)
            {
                var newMailCC = string.Empty;
                var newMailBcc = string.Empty;
                var mailingListCriteria = new GetMailingListByFunctionKeyAndSiteCriteria
                {
                    FunctionKey = functionKey,
                    SiteId = NapSettings.Settings.SiteId,
                    IsInternal = true
                };

                // List mail IT
                mailTo = this.GetNewValue(mailTo, this.MailTo(mailingListCriteria, out newMailCC, out newMailBcc));
                mailCC = this.GetNewValue(mailCC, newMailCC);
                mailBcc = this.GetNewValue(mailBcc, newMailBcc);
            }

            outMailBcc = mailBcc;
            outMailCC = mailCC;
            return mailTo;
        }

        /// <summary>
        /// Get mailing list for CSD.
        /// </summary>
        /// <param name="voidBetEmailViewModel">View model value.</param>
        /// <param name="outMailCC">Out CC mail address value.</param>
        /// <param name="outMailIt">Out Bcc mail address value.</param>
        /// <returns>Return list mail To.</returns>
        public string GetMailCSD(VoidBetEmailViewModel voidBetEmailViewModel, out string outMailCC, out string outMailIt)
        {
            var mailCC = string.Empty;
            var mailCsd = string.Empty;
            var mailIt = string.Empty;

            // Loop over function keys.
            foreach (var functionKey in voidBetEmailViewModel.FunctionName)
            {
                var newMailCC = string.Empty;
                var newMailBcc = string.Empty;
                var newMailIt = string.Empty;
                var mailingListCriteria = new GetMailingListByFunctionKeyAndSiteCriteria
                {
                    FunctionKey = functionKey,
                    SiteId = NapSettings.Settings.SiteId,
                    IsInternal = false
                };

                // List mail CSD
                mailCsd = this.GetNewValue(mailCsd, this.MailTo(mailingListCriteria, out newMailCC, out newMailBcc));
                mailCC = this.GetNewValue(mailCC, newMailCC);

                // List mail IT
                mailingListCriteria.IsInternal = true;
                newMailIt = this.MailTo(mailingListCriteria);
                newMailIt = string.IsNullOrEmpty(newMailIt) ? newMailBcc : this.GetNewValue(newMailIt, newMailBcc);
                mailIt = this.GetNewValue(mailIt, newMailIt);
            }

            outMailIt = mailIt;
            outMailCC = mailCC;
            return mailCsd;
        }
    }
}