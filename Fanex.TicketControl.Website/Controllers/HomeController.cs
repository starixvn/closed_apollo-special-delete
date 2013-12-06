// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.25">Created</change>
// </history>

namespace Fanex.TicketControl.Website.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.TicketControl.Core.Web.Controllers;

    /// <summary>
    /// Home page controller.
    /// </summary>
    [NapSuperAuth]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult _Menu()
        {
            ViewBag.MenuItemList = CurrentSession.Menu;
            return PartialView("~/Views/Shared/_Menu.cshtml");
        }
    }
}