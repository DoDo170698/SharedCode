using QuanLyHoaDon.CodeLogic;
using QuanLyHoaDon.CodeLogic.Attributes;
using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.Views;
using QuanLyHoaDon.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QuanLyHoaDon.CodeLogic.Enums.Enums;

namespace QuanLyHoaDon.Controllers
{
    public class HomeController : BaseController
    {
        //[AuthorizeCustom(Modules = new int[] { (int)IModule.QLHD_Manage })]
        public ActionResult Index()
        {
            return Redirect("/order?ViewName=Receive");
        }
        [HttpGet]
        public ActionResult Login()
        {
            SetUserLogin();
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(Account user)
        {
            SetUserSession(user);
            return Redirect("/order?ViewName=Receive");
        }
        public ActionResult LogOut(Account user)
        {
            DestroySessionUser(user);
            return RedirectToAction("Login");
        }
        protected void SetUserSession(Account user)
        {
            Session["CurrentUser"] = user;
            ViewBag.CUser = user;
        }
        protected void SetUserLogin()
        {
            ViewBag.LoginMessage = Session["LoginMessage"];
            ViewBag.CUser = Session["CurrentUser"];
        }
        protected void DestroySessionUser(Account user)
        {
            Session["CurrentUser"] = null;
            Session["LoginMessage"] = null;
        }
    }
}