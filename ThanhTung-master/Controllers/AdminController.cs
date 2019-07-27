using NPoco;
using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyHoaDon.Controllers
{
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            var accounts = Account.UseInstance.GetListOrDefault();
            SetTitle("Quản lý tài khoản");
            return GetCustResultOrView(new ViewParam {
                ViewName ="Index",
                ViewNameAjax ="Admins",
                Data = new AdminModel
                {
                    Accounts = accounts,
                }
            });
        }
        public ActionResult Create()
        {
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Create",
                ViewNameAjax = "Create",
                Data = null
            }); ;
        }

    }
}