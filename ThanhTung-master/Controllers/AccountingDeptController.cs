using QuanLyHoaDon.CodeLogic;
using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.PurchaseManage;
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
    public class AccountingDeptController : BaseController
    {
        // GET: AccountingDept
        string defauthPath = "AccountingDept";

        public ActionResult Index()
        {
            var searchParam = new AccountingDeptParam().BindData(DATA);
            var accountingDepts = AccountingDeptRepository.Search(searchParam, Paging);
            var idOrders = new long[] { 0 } ;
            idOrders = Equals(accountingDepts, null)
                     ? idOrders
                     : accountingDepts.Select(t => (long)t.IDOrder).ToArray();
            var orders = OrderRepository.UseInstance.GetByIdsOrDefault(idOrders);
            var orderFiles = OrderFileRepository.UseInstance.GetListByFieldsOrDefault(new List<CondParam> {
                new CondParam
                {
                    Field="OrderStatus", Value = (int)OrderStatus.CollectVouchers,CompareType = (int)CompareTypes.Equal, TypeSQL= (int)TypeSQl.Number
                },
                new CondParam
                {
                    Field = "IDOrder", Value = orders.Select(t=>(long)t.ID).ToArray(), CompareType = (int)CompareTypes.In, TypeSQL = (int)TypeSQl.Number,
                }
            });
            var listStatus = Utils.EnumToDictionary<AccountingDeptStatus>();
            SetTitle("Danh sách đơn hàng gửi đến phòng kế toán");
            return GetCustResultOrView(new ViewParam
            {

                ViewName = "Index",
                ViewNameAjax = "AccountingDepts",
                Data = new AccountingDeptModel
                {
                    Orders = orders,
                    OrderFiles = orderFiles,
                    AccountingDepts = accountingDepts,
                    ListStatus = listStatus,
                    SearchParam = searchParam,
                },
            });
        }
        public ActionResult Create()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return Redirect("/order");
            }
            var type = Utils.GetInt(DATA,"Type");
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    ID = id,
                    Title = string.Format("Phòng kế toán"),
                    Content = string.Format("Bạn có xác nhận chuyển đến phòng kế toán cho đơn [{0}]?", order.PONo),
                    Url = string.Format("/AccountingDept/Save?Type={0}",type),
                    Class = "btn-violet white",
                },
                ViewName = "~/Views/Shared/IsConfirm.cshtml",
                ViewNameAjax = "~/Views/Shared/IsConfirm.cshtml",
                Width = 500
            });
        }
        public ActionResult Save()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return Redirect("/order");
            }
            order.Status = (int)OrderStatus.Payed;
            order.PayedDate = DateTime.Now;
            order.AccountingDeptDate = DateTime.Now;
            var type = Utils.GetInt(DATA,"Type");
            var accountingDpet = new AccountingDept
            {
                IDChannel = 0,
                IDOrder = order.ID,
                Status = (int)AccountingDeptStatus.Receive,
                Created = DateTime.Now,
                CreatedBy = 0,
                Type = type
            };

            if (AccountingDeptRepository.UseInstance.Insert(accountingDpet) && OrderRepository.UseInstance.Update(order))
            {
                SetSuccess(string.Format("Xác nhận chuyển đến phòng kế toán của đơn [{0}] thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Xác nhận thu thập chứng từ của đơn [{0}] không thành công", order.PONo));
            }

            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult Update()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return Redirect("/order");
            }
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    ID = id,
                    Title = string.Format("Phòng kế toán"),
                    Content = string.Format("Bạn có xác nhận chuyển đến phòng kế toán cho đơn [{0}]?", order.PONo),
                    Url = "/AccountingDept/Change",
                    Class = "btn-violet white",
                },
                ViewName = "~/Views/Shared/IsConfirm.cshtml",
                ViewNameAjax = "~/Views/Shared/IsConfirm.cshtml",
                Width = 500
            });
        }
        public ActionResult Change()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetByIdOrDefault(id);
            if (Equals(order, null))
            {
                SetError("Thông tin hóa đơn không còn tồn tại");

            }
            var accountingDept = AccountingDeptRepository.UseInstance.GetFirstByFields(new List<CondParam> {
                new CondParam
                {
                    Field = "IDOrder", Value = order.ID, CompareType = (int)CompareTypes.Equal, TypeSQL = (int)TypeSQl.Number
                }
            });
            order.Status = (int)OrderStatus.Payed;
            accountingDept.Status = (int)AccountingDeptStatus.Receive;
            try
            {
                if (AccountingDeptRepository.UseInstance.Update(accountingDept) && OrderRepository.UseInstance.Update(order))
                {
                    SetSuccess(string.Format("Chuyển hóa đơn [{0}] sang Phòng kế toán thành công",order.PONo));
                }
                else
                {
                    SetError(string.Format("Chuyển hóa đơn [{0}] sang Phòng kế toán không thành công", order.PONo));
                }
            }
            catch (Exception e)
            {
                var mess = e.Message;
                SetError(string.Format("Chuyển hóa đơn [{0}] sang Phòng kế toán không thành công", order.PONo));
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsConfirm()
        {
            var id = Utils.GetInt(DATA, "ID");
            var accountingDept = AccountingDeptRepository.UseInstance.GetById(id);
            var order = OrderRepository.UseInstance.GetById(accountingDept.IDOrder);
            if (Equals(accountingDept, null) || Equals(order, null))
            {
                SetError("Thông tin đơn hàng không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    ID = id,
                    Title = string.Format("Xác nhận chứng từ hợp lệ"),
                    Content = string.Format("Bạn có xác nhận chứng từ cho đơn [{0}]?", order.PONo),
                    Url = "/AccountingDept/Confirm",
                    Class = "btn-blueviolet  white",
                },
                ViewName = "~/Views/Shared/IsConfirm.cshtml",
                ViewNameAjax = "~/Views/Shared/IsConfirm.cshtml",
                Width = 500
            });
        }
        public ActionResult Confirm()
        {
            var id = Utils.GetInt(DATA, "ID");
            var accountingDept = AccountingDeptRepository.UseInstance.GetById(id);
            var order = OrderRepository.UseInstance.GetByIdOrDefault(accountingDept.IDOrder);
            if (Equals(accountingDept, null) || Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.AccountingDeptConfirmDate = DateTime.Now;
            accountingDept.Status = (int)AccountingDeptStatus.Confirm;
            accountingDept.ConfirmDate = DateTime.Now;

            if (OrderRepository.UseInstance.Update(order) && AccountingDeptRepository.UseInstance.Update(accountingDept))
            {
                SetSuccess(string.Format("Xác nhận thu thập chứng từ của đơn [{0}] thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Xác nhận thu thập chứng từ của đơn [{0}] không thành công", order.PONo));
            }
            return GetResultOrReferrerDefault(defauthPath);
        }

        public ActionResult IsReject()
        {
            var id = Utils.GetInt(DATA, "ID");
            var accountingDept = AccountingDeptRepository.UseInstance.GetById(id);
            var order = OrderRepository.UseInstance.GetById(accountingDept.IDOrder);
            if (Equals(order, null) || Equals(accountingDept, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    ID = id,
                    Title = string.Format("Từ chối xác nhân thu thập chứng từ"),
                    Content = string.Format("Bạn có muốn từ chối xác nhận thu thập chứng từ cho đơn [{0}]?", order.PONo),
                    Url = "/AccountingDept/Reject",
                    Class = "btn-azure white",
                },
                ViewName = "~/Views/Shared/IsConfirm.cshtml",
                ViewNameAjax = "~/Views/Shared/IsConfirm.cshtml",
                Width = 500
            });
        }
        public ActionResult Reject()
        {
            var id = Utils.GetInt(DATA, "ID");
            var accountingDept = AccountingDeptRepository.UseInstance.GetById(id);
            var order = OrderRepository.UseInstance.GetById(accountingDept.IDOrder);
            if (Equals(order, null) || Equals(accountingDept, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = (int)OrderStatus.CollectVouchers;
            order.AccountingDeptRejectDate = DateTime.Now;
            accountingDept.Status = (int)AccountingDeptStatus.Reject;
            accountingDept.RejectDate = DateTime.Now;

            if (OrderRepository.UseInstance.Update(order) && AccountingDeptRepository.UseInstance.Update(accountingDept))
            {
                SetSuccess(string.Format("Xác nhận chuyển đến phòng kế toán của đơn [{0}] thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Xác nhận thu thập chứng từ của đơn [{0}] không thành công", order.PONo));
            }

            return GetResultOrReferrerDefault(defauthPath);
        }

    }
}