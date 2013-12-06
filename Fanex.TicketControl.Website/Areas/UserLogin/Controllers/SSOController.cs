// --------------------------------------------------------------------------
// <copyright file="SSOController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.08.29">Add header</change>
// </history>

namespace Fanex.TicketControl.Website.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Web;
    using Fanex.TicketControl.Core.Web.Controllers;

    /// <summary>
    /// SSO Controller.
    /// </summary>
    public class SSOController : BaseController
    {
        #region members

        private readonly int _timeExipration = 30;
        private readonly IUserService _userService;
        private readonly IMenuItemService _menuItemService;
        private readonly ICurrentSession _currentSession;
        private readonly IFunctionService _functionService;

        #endregion members

        #region ctors

        public SSOController()
            : this(null, null, null, null)
        {
        }

        public SSOController(
            IUserService userService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
            IFunctionService functionService)
        {
            _userService = userService ?? new UserService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
            _functionService = functionService ?? new FunctionService();
        }

        #endregion ctors

        /// <summary>
        /// SSO login.
        /// </summary>
        /// <param name="userId">User id value.</param>
        /// <param name="onuserId">On user id value.</param>
        /// <param name="time">Time value.</param>
        /// <returns>Redirect page.</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string userId, string onuserId, string time)
        {
            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserId = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            string currentTime = _userService.UserSSOGetTime();

            var expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= _timeExipration)
            {
                var criteria = SetUserCriteria(Convert.ToInt32(userId), Convert.ToInt32(onuserId));
                _userService.UserSSOCheckIn(ref criteria);

                if (criteria.OnUserId != 0)
                {
                    var user = _userService.GetById(criteria.UserId) as User;

                    _currentSession.UserId = user.Id;
                    _currentSession.Username = user.Username;
                    _currentSession.OnUserId = criteria.OnUserId;
                    _currentSession.SessionId = Session.SessionID.ToString();
                    _currentSession.Level = _userService.GetUserSiteLevel(user.Id, this.SiteId);

                    if (!ReferenceEquals(user.Group, null))
                    {
                        _currentSession.GroupId = user.Group.Id;
                        _currentSession.GroupName = user.Group.Name;
                    }

                    var currentFunction = _functionService.GetByUserAndSiteIncludeGroup(user.Id, NapSettings.Settings.SiteId, NapSettings.Settings.Environment);
                    AppSession.CurrentUser = user;
                    AppSession.RoleAccess = PermissionService.GetRoleAccess(currentFunction);

                    if (this.SiteId != 0)
                    {
                        _currentSession.Menu = _menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                user.Id,
                                                                                SiteId,
                                                                                Environment,
                                                                                true) as IList<MenuItem>;
                        AppSession.CurrentUserMenu = _currentSession.Menu;
                    }

                    CreateAuthInfo(user);
                    return Redirect(FormsAuthentication.DefaultUrl);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        #region utilities

        /// <summary>
        /// Set new value to user criteria.
        /// </summary>
        /// <param name="userId">User id parameter.</param>
        /// <param name="onuserId">On User id.</param>
        /// <returns>Return UserCriteria.</returns>
        private UserSSOCheckInUserCriteria SetUserCriteria(int userId, int onuserId)
        {
            return new UserSSOCheckInUserCriteria
            {
                UserId = userId,
                SiteId = SiteId,
                CheckOnUserId = onuserId,
                OnUserId = 0,
                IP = CurrentContext.IP,
                NetSessionId = Session.SessionID,
                LoginDate = DateTime.Now,
                BrowserName = CurrentContext.BrowserName,
                CountryCode = CurrentContext.CountryCode,
                Domain = CurrentContext.DomainName,
                Platform = CurrentContext.Platform
            };
        }

        /// <summary>
        /// Save cookie user info.
        /// </summary>
        /// <param name="user">User info.</param>
        private void CreateAuthInfo(IUser user)
        {
            var currentDomain = CurrentContext.GetCurrentDomain();

            var authInfo = new AuthInfo { UserId = user.Id, OnUserId = this._currentSession.OnUserId };

            var authTicket = new FormsAuthenticationTicket(
                 1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        #endregion utilities
    }
}