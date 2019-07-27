using QuanLyHoaDon.CodeLogic;
using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.CodeLogic.Customs;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.PurchaseManage;
using QuanLyHoaDon.Models.Views;
using QuanLyHoaDon.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QuanLyHoaDon.CodeLogic.Enums.Enums;

namespace QuanLyHoaDon.Controllers
{
    public class ExportController : BaseController
    {
        public ActionResult OrderDiscountExportPDF()
        {
            var path = "";
            var temp = "OrderDiscount.pdf";
            var fileName = "Danh sách chiết khấu.pdf";
            var dataTable = new DataTable();
            dataTable.TableName = string.Format("Danh sách chiết khấu");
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "STT",
                Caption = "Num"
            }
           );
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Thông tin đơn chiết khấu",
                Caption = "OrderInfo"
            }
            );
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Chi tiết đơn hàng",
                Caption = "DetailOrders"
            });
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Chiết khấu(%)",
                Caption = "Discount"
            });
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Hỗ trợ Event(VNĐ)",
                Caption = "Event"
            });
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Tổng tiền(VNĐ)",
                Caption = "Total"
            });

            var searchParm = new OrderDiscountParam().BindData(DATA);
            var orderDiscounts = OrderDiscountRepository.Search(searchParm, null);
            var idOrderString = Utils.GetStringJoin(",", orderDiscounts.Select(t => t.IDOrders));
            var idOrders = idOrderString.ToLongSplit(',').Distinct().ToArray();
            var quaters = Utils.EnumToDictionary<Quater>();
            var years = Enumerable.Range(2000, 100).ToList();
            var orders = OrderRepository.UseInstance.GetByIdsOrDefault(idOrders);
            var customers = CustomerRepository.UseInstance.GetListOrDefault();
            var orderTypes = Utils.EnumToDictionary<OrderType>();

            var i = 0;
            orderDiscounts.ForEach(t =>
            {
                var customer = customers.FirstOrDefault(c => c.ID == t.IDCustomer) ?? new Customer();
                var idOrderFors = t.IDOrders.ToIntSplit(',');
                var orderFors = orders.Where(o => idOrderFors.Contains(o.ID)).ToList();
                var time = t.Quater != (int)(Quater.All)
                         ? string.Format("{0} - {1}", Utils.GetDescription<Quater>(t.Quater), t.Year)
                         : string.Format("Năm {0}", t.Year);
                var orderDetails = "";
                orderFors.ForEach(o =>
                {
                    orderDetails += string.Format("{0}: {1}", o.PONo, o.RealValue > 0 ? string.Format("{0:0,0}", o.RealValue) : "0") + Environment.NewLine;
                });
                var sumOrderDetails = orderFors.Sum(o => o.RealValue);
                orderDetails += string.Format("Tổng :{0}", sumOrderDetails > 0 ? string.Format("{0:0,0}", sumOrderDetails) : "0");
                i++;
                dataTable.Rows.Add(
                    i,
                      string.Format("Mã CK: {0}", t.Code) + Environment.NewLine
                    + string.Format("Khách hàng: {0}", customer.Name) + Environment.NewLine
                    + string.Format("Thời điểm: {0}", time) + Environment.NewLine
                    + string.Format("Loại đơn: {0}", Utils.GetDescription<OrderType>(t.Type)),
                    orderDetails,
                    t.Percent,
                    t.Event > 0 ? string.Format("{0:0,0}", t.Event) : "0",
                    t.RealTotal > 0 ? string.Format("{0:0,0}", t.RealTotal) : "0"
                );
            });
            var widths = new float[] { 30f, 150f, 150f, 50f, 90f, 90f };
            var header = string.Format("Danh sách chiết khấu");
            var footer = string.Format("Có tất cả {0} đơn chiết khấu", orderDiscounts.Count);
            CUtils.ExportToPdf(dataTable, temp, out path, header, footer, widths);
            return FileResult(path, fileName);
        }
        public ActionResult OrderExportPDF()
        {
            var path = "";
            var temp = "Order.pdf";
            var fileName = "Danh sách đơn hàng.pdf";
            var searchParam = new SearchParam().BindData(DATA);
            searchParam.Type = Utils.GetInt(DATA, "Type");
            var allStatusParams = Utils.GetInts(DATA, "ListStatus", IsAjax);
            var orders = OrderRepository.Search(searchParam, null) ?? new List<Order>();
            var listStatus = Utils.EnumToDictionary<OrderStatus>();
            var viewName = Utils.GetString(DATA, "ViewName").ToLower();
            var adOrders = new List<int>();
            if (Equals(viewName, "Payed".ToLower()))
            {
                adOrders = AccountingDeptRepository.UseInstance.GetListByFieldsOrDefault(new List<CondParam> {
                    new CondParam
                    {
                        Field ="Status", Value = (int)AccountingDeptStatus.Confirm, TypeSQL = (int)TypeSQl.Number, CompareType = (int)CompareTypes.Equal
                    }
                }).Select(t => t.IDOrder).ToList();
            }
            else if (Equals("CollectVouchers".ToLower(), viewName))
            {
                adOrders = AccountingDeptRepository.UseInstance.GetListByFieldsOrDefault(new List<CondParam> {
                    new CondParam
                    {
                        Field ="Status", Value = (int)AccountingDeptStatus.Reject, TypeSQL = (int)TypeSQl.Number, CompareType = (int)CompareTypes.Equal
                    }
                }).Select(t => t.IDOrder).ToList();
            }
            var dataTable = new DataTable();
            dataTable.TableName = string.Format("Danh sách đơn hàng");
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "STT",
                Caption = "Num"
            }
          );
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Tên đơn hàng",
                Caption = "Name"
            }
            );
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Thông tin đặt hàng",
                Caption = "OrderInfo"
            });
            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Người đặt hàng",
                Caption = "Discount"
            });

            dataTable.Columns.Add(new DataColumn
            {
                ColumnName = "Trạng thái",
                Caption = "Status"
            });
            var widths = new float[] { 30f, 70f, 150f, 150f, 100f };
            var header = string.Format("Danh sách đơn hàng");
            var footer = string.Format("Có tất cả {0} đơn hàng", orders.Count);
            var i = 0;
            orders.ForEach(item =>
            {
                i++;
                var poDate = "";
                var deliveryDate = "";
                if (!Equals(item.PODate.Value, null))
                {
                    poDate = string.Format("{0} {1}", item.PODate.Value.ToString("dd-MM-yyyy"), item.PODate.Value.ToString("HH:mm"));
                }
                if (!Equals(item.DeliveryDate.Value, null))
                {
                    deliveryDate = string.Format("{0} {1}", item.DeliveryDate.Value.ToString("dd-MM-yyyy"), item.DeliveryDate.Value.ToString("HH:mm"));
                }

                dataTable.Rows.Add(
                    i,
                    item.PONo,
                    string.Format("Ngày đặt hàng: {0}", poDate) + Environment.NewLine
                  + string.Format("Ngày giao hàng: {0}", deliveryDate) + Environment.NewLine
                  + string.Format("Địa chỉ giao hàng: {0}", item.Address),
                    string.Format("Tên: {0}", item.Purchaser) + Environment.NewLine
                  + string.Format("SĐT: {0}", item.Tel) + Environment.NewLine
                  + string.Format("Email: {0}", item.Email),
                    Utils.GetDescription<OrderStatus>(item.Status)
                );
            });

            CUtils.ExportToPdf(dataTable, temp, out path, header, footer, widths);
            return FileResult(path, fileName);
        }
    }
}