using QuanLyHoaDon.CodeLogic;
using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
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
    public class OrderDiscountController : BaseController
    {
        string defauthPath = "/OrderDiscount";
        #region OrderDiscount
        public ActionResult Index()
        {
            var searchParam = new OrderDiscountParam().BindData(DATA);
            var orderDiscounts = OrderDiscountRepository.Search(searchParam, Paging);
            var idOrderString = Utils.GetStringJoin(",", orderDiscounts.Select(t => t.IDOrders));
            var idOrders = idOrderString.ToLongSplit(',').Distinct().ToArray();
            var quaters = Utils.EnumToDictionary<Quater>();
            var years = Enumerable.Range(2000, 100).ToList();
            var orders = OrderRepository.UseInstance.GetByIdsOrDefault(idOrders);
            var customers = CustomerRepository.UseInstance.GetListOrDefault();
            var orderTypes = Utils.EnumToDictionary<OrderType>();
            SetTitle("Danh sách chiết khấu");
            return GetCustResultOrView(new ViewParam
            {
                Data = new OrderDiscountModel
                {
                    OrderDiscounts = orderDiscounts,
                    Quaters = quaters,
                    Years = years,
                    Customers = customers,
                    Orders = orders,
                    OrderTypes = orderTypes,
                    SearchParam = searchParam,
                },
                ViewName = "Index",
                ViewNameAjax = "OrderDiscounts"
            });
        }
        public ActionResult Create()
        {
            var quaters = Utils.EnumToDictionary<Quater>();
            var years = Enumerable.Range(2000, 100).ToList();
            var customers = CustomerRepository.UseInstance.GetListOrDefault();
            var orderTypes = Utils.EnumToDictionary<OrderType>();

            SetTitle("Tạo mới đon chiết khấu");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Create",
                ViewNameAjax = "Create",
                Data = new OrderDiscountModel
                {
                    Quaters = quaters,
                    OrderDiscount = new OrderDiscount
                    {
                        Year = DateTime.Now.Year,
                        Quater = Utils.Quater(DateTime.Now)
                    },
                    OrderTypes = orderTypes,
                    Customers = customers,
                    Years = years,
                    Url = "/OrderDiscount/save",
                },
                Width = 700
            });
        }
        public ActionResult Save()
        {
            var orderDiscount = new OrderDiscount().BindData(DATA);
            if (!IsValidate(orderDiscount))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            var eventMoney = Utils.GetString(DATA, "EventMoney").Replace(",", string.Empty);
            var totalMoney = Utils.GetString(DATA, "TotalMoney").Replace(",", string.Empty);
            var realTotalMoney = Utils.GetString(DATA, "RealTotalMoney").Replace(",", string.Empty);
            var idOrders = Utils.GetInts(DATA, "IDOrder", IsAjax);

            orderDiscount.Event = long.Parse(eventMoney);
            orderDiscount.Total = long.Parse(totalMoney);
            orderDiscount.RealTotal = long.Parse(realTotalMoney);
            orderDiscount.IDOrders = Utils.GetStringJoin(",", idOrders);
            if (OrderDiscountRepository.UseInstance.Insert(orderDiscount))
            {
                SetSuccess("Tạo đơn chiết khấu hóa thành công");
            }
            else
            {
                SetError("Tạo mới đon chiết khấu không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult Update(int id = 0)
        {
            var orderDiscount = OrderDiscountRepository.UseInstance.GetById(id);
            if (Equals(orderDiscount, null))
            {
                SetError("Thông tin đơn chiết khấu không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            var quaters = Utils.EnumToDictionary<Quater>();
            var years = Enumerable.Range(2000, 100).ToList();
            var customers = CustomerRepository.UseInstance.GetListOrDefault();
            var orderTypes = Utils.EnumToDictionary<OrderType>();

            var idOrders = orderDiscount.IDOrders.ToLongSplit(',').Distinct().ToArray();
            var orders = OrderRepository.UseInstance.GetByIdsOrDefault(idOrders);
            SetTitle("Cập nhật đơn chiết khấu");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Data = new OrderDiscountModel
                {
                    OrderDiscount = orderDiscount,
                    Quaters = quaters,
                    Years = years,
                    Customers = customers,
                    Orders = orders,
                    OrderTypes = orderTypes,
                    Url = "/OrderDiscount/Change"
                },
                Width = 700
            });
        }
        public ActionResult Change()
        {
            var id = Utils.GetInt(DATA, "ID");
            var orderDiscount = OrderDiscountRepository.UseInstance.GetById(id);
            if (!IsValidate(orderDiscount))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            orderDiscount.BindData(DATA, false);
            var eventMoney = Utils.GetString(DATA, "EventMoney").Replace(",", string.Empty);
            var totalMoney = Utils.GetString(DATA, "TotalMoney").Replace(",", string.Empty);
            var realTotalMoney = Utils.GetString(DATA, "RealTotalMoney").Replace(",", string.Empty);
            var idOrders = Utils.GetInts(DATA, "IDOrder", IsAjax);

            orderDiscount.Event = long.Parse(eventMoney);
            orderDiscount.Total = long.Parse(totalMoney);
            orderDiscount.RealTotal = long.Parse(realTotalMoney);
            orderDiscount.IDOrders = Utils.GetStringJoin(",", idOrders);
            if (OrderDiscountRepository.UseInstance.Update(orderDiscount))
            {
                SetSuccess("Chỉnh sửa thông tin hàng hóa thành công");
            }
            else
            {
                SetError("Chỉnh sửa thông tin hàng hóa không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsDelete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var orderDiscount = OrderDiscountRepository.UseInstance.GetById(id);
            if (Equals(orderDiscount, null))
            {
                SetError("Thông tin đơn chiết khấu không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    ID = id,
                    //   Class ="",
                    //Target = ".ui-dialog:visible",
                    Title = string.Format("Xóa thông tin hàng hóa"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin đơn chiết khấu [{0}]?", orderDiscount.Code),
                    Url = "/OrderDiscount/delete"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 500
            });
        }
        public ActionResult Delete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var OrderDiscount = OrderDiscountRepository.UseInstance.GetById(id);
            if (Equals(OrderDiscount, null))
            {
                SetError("Thông tin đơn chiết khấu không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (OrderDiscountRepository.UseInstance.Delete(OrderDiscount.ID))
            {
                SetSuccess(string.Format("Xóa thông tin của đơn chiết khấu [{0}] thành công", OrderDiscount.Code));
            }
            else
            {
                SetError(string.Format("Xóa thông tin của đơn chiết khấu [0] không thành công"));
            }

            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsDeletes()
        {

            var ids = Utils.GetString(DATA, "ID[]").Split(',');
            if (!ids.Any())
            {
                SetError("Bạn chưa chọn thông tin nào để xóa");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    //  IDs = ids,
                    IDs = ids.Serialize(),
                    Title = string.Format("Xóa thông tin nhiều đơn chiết khấu"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin của [{0}] đơn chiết khấu?", ids.Length),
                    IsMulti = true,
                    Url = "/OrderDiscount/deletes"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 500
            });
        }
        public ActionResult Deletes()
        {
            var ids = Utils.GetString(DATA, "IDs").DeSerialize<long[]>();
            if (!ids.Any())
            {
                SetError("Bạn chưa chọn thông tin nào để xóa");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (OrderDiscountRepository.UseInstance.Deletes(ids))
            {
                SetSuccess(string.Format("Xóa [{0}] đơn chiết khấu thành công", ids.Length));
            }
            else
            {
                SetError(string.Format("Xóa [{0}] đơn chiết khấu không thành công", ids.Length));
            }

            return GetResultOrReferrerDefault(defauthPath);
            // var
        }
        private bool IsValidate(OrderDiscount orderDiscount)
        {
            if (orderDiscount.IDCustomer <= 0)
            {
                SetError("Bạn chưa chọn khách hàng");
            }
            return !HasError;
        }
        public ActionResult GetOrder()
        {
            var idCustomer = Utils.GetInt(DATA, "IDCustomer");
            var year = Utils.GetInt(DATA, "Year");
            var quater = Utils.GetInt(DATA, "Quater");
            var type = Utils.GetInt(DATA, "Type");
            var months = Utils.GetMonthOfQuater(quater);
            var stringMonths = Utils.GetStringJoin(",", months);
            var orders = OrderRepository.UseInstance.GetListByFieldsOrDefault(new List<CondParam> {
                new CondParam
                {
                    Field = "IDCustomer", Value = idCustomer, TypeSQL = (int)TypeSQl.Number, CompareType= (int)CompareTypes.Equal
                },
                 new CondParam
                {
                    Field = "Type", Value = type, TypeSQL = (int)TypeSQl.Number, CompareType= (int)CompareTypes.Equal
                },
                 new CondParam
                 {
                     Sql = string.Format(" YEAR(PayedConfirmDate) ={0} AND MONTH(PayedConfirmDate) in ({1})",year,stringMonths)
                 }
            });
            var idOrderDiscount = Utils.GetInt(DATA, "IDOrderDiscount");

            _addRes = new
            {
                Total = orders.Sum(t => t.RealValue),
                InputValue = "#input_value_" + idOrderDiscount,
                DetailOrder = "#table_order_detail",
                IDTotal = "#Total_" + idOrderDiscount
            };
            return GetCustResultOrView(new ViewParam
            {

                ViewNameAjax = "Orders",
                Data = new OrderDiscountModel
                {
                    Orders = orders
                }
            });
        }
        #endregion
    }
}