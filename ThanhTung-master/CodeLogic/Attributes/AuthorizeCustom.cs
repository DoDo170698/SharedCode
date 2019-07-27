using QuanLyHoaDon.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyHoaDon.CodeLogic.Attributes
{
    public class AuthorizeCustom : AuthorizeAttribute
    {
        public int[] Modules { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Equals(filterContext.HttpContext.Session["CurrentUser"], null))
            {
                filterContext.HttpContext.Session["CurrentUser"] = new Account();
            }
            base.OnAuthorization(filterContext);

        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return IsAuthorize(httpContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result =
               new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                        { "action", "login" },
                        { "controller", "home" }
                   });
        }
        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return base.OnCacheAuthorization(httpContext);
        }
        private bool IsUserExist(List<Account> baseUsers, ref Account user, ref string message)
        {
            try
            {
                var isAuthorize = false;
                var userCheck = user;
                if (!Equals(baseUsers, null))
                {
                    var userInDB = baseUsers.FirstOrDefault(t =>
                                          t.UserName.ToLower() == userCheck.UserName.ToLower()
                                       && t.PassWord.ToLower() == userCheck.PassWord.ToLower());

                    if (Equals(userInDB, null))
                    {
                        message = string.Format("Sai tài tên tài khoản hoặc mật khẩu");
                        return false;
                    }
                    else
                    {
                        user = userInDB;
                        var roles = userInDB.Roles.Split(',').Select(t => int.Parse(t));

                        if (userInDB.IsAdmin)
                        {
                            isAuthorize = true;
                        }
                        else
                        {
                            for (int i = 0; i < Modules.Length; i++)
                            {
                                if (roles.Contains(Modules[i]))
                                {
                                    isAuthorize = true;
                                }
                            }
                        }
                        if (isAuthorize)
                        {
                            message = string.Format("Đăng nhập thành công");
                        }
                        else
                        {
                            message = string.Format("Bạn không có quyền truy cập Module này");
                        }
                    }

                }
                return isAuthorize;
            }
            catch
            {
                return false;
            }

        }
        private bool IsAuthorize(HttpContextBase httpContext)
        {
            try
            {
                var isAuthorize = false;
                var baseUsers = (List<Account>)httpContext.Session["Base_Users"];
                var currrentUser = (Account)httpContext.Session["CurrentUser"];
                var message = "";
                isAuthorize = IsUserExist(baseUsers, ref currrentUser, ref message);
                httpContext.Session["LoginMessage"] = message;
                httpContext.Session["CurrentUser"] = currrentUser;
                return isAuthorize;
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return false;
            }

        }

    }
}