using QuanLyHoaDon.CodeLogic;
using QuanLyHoaDon.CodeLogic.Attributes;
using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.CodeLogic.Customs;
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
    public class OrderController : BaseController
    {
        string defauthPath = "/Order?ViewName=Receive";
        #region Order
        //[AuthorizeCustom(Modules = new int[] { (int)IModule.QLHD_Manage })]
        public ActionResult Index()
        {
            var searchParm = new SearchParam().BindData(DATA);
            searchParm.Type = (int)OrderType.Order;
            Paging.PageSize = 5;
            var allStatusParams = Utils.GetInts(DATA, "ListStatus", IsAjax);
            var orders = OrderRepository.Search(searchParm, Paging) ?? new List<Order>();
            var listStatus = Utils.EnumToDictionary<OrderStatus>();
            var viewName = Utils.GetString(DATA, "ViewName");
            viewName = viewName.IsNullOrEmpty() ? "Receive" : viewName;
            searchParm.ViewName = viewName;
            var adOrders = new List<int>();
            if (Equals(viewName, "Payed"))
            {
                adOrders = AccountingDeptRepository.UseInstance.GetListByFieldsOrDefault(new List<CondParam> {
                    new CondParam
                    {
                        Field ="Status", Value = (int)AccountingDeptStatus.Confirm, TypeSQL = (int)TypeSQl.Number, CompareType = (int)CompareTypes.Equal
                    }
                }).Select(t => t.IDOrder).ToList();
            }
            else if (Equals("CollectVouchers", viewName))
            {
                adOrders = AccountingDeptRepository.UseInstance.GetListByFieldsOrDefault(new List<CondParam> {
                    new CondParam
                    {
                        Field ="Status", Value = (int)AccountingDeptStatus.Reject, TypeSQL = (int)TypeSQl.Number, CompareType = (int)CompareTypes.Equal
                    }
                }).Select(t => t.IDOrder).ToList();
            }
            SetTitle(CUtils.GetTitleByView(viewName));
            return GetCustResultOrView(new ViewParam
            {
                Data = new OrderModel
                {
                    Orders = orders,
                    ListStatus = listStatus,
                    SearchParam = searchParm,
                    IDOrders = adOrders,
                },
                ViewName = "Index",
                ViewNameAjax = "Orders"
            });
        }
        public ActionResult Create()
        {
            var products = ProductRepository.UseInstance.GetListOrDefault();
            SetTitle("Tạo mới đơn");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Create",
                ViewNameAjax = "Create",
                Width = 1000,
                Data = new OrderModel
                {
                    Products = products,
                    Order = new Order { Status = (int)OrderStatus.Receive },
                    Status = (int)OrderStatus.Receive,
                    Customer = new Customer(),
                    Supplier = new Supplier(),
                    Url = "/Order/save"
                }
            }); ;
        }
        public ActionResult Save()
        {
            var order = new Order().BindData(DATA);
            if (!IsValidate(order))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            var valueOfOrder = long.Parse(Utils.GetString(DATA, "ValueOfOrder").Replace(",", ""));
            order.Value = valueOfOrder;
            order.RealValue = valueOfOrder;
            order.Status = (int)OrderStatus.Receive;
            order.Created = DateTime.Now;
            order.Type = (int)OrderType.Order;
            var idProducts = Utils.GetInts(DATA, "ProductID", IsAjax);
            var amounts = Utils.GetStrings(DATA, "ProductAmount", IsAjax);
            var promotions = Utils.GetStrings(DATA, "ProductPromotion", IsAjax);
            var totals = Utils.GetStrings(DATA, "Total", IsAjax);
            var productOrders = new List<ProductOrder>();
            if (!idProducts.IsNullOrEmpty())
            {
                for (int i = 0; i < idProducts.Length; i++)
                {
                    productOrders.Add(new ProductOrder
                    {
                        IDChannel = 0,
                        IDOrder = order.ID,
                        IDProduct = idProducts[i],
                        Amount = int.Parse(amounts[i].Replace(",", "")),
                        Total = long.Parse(totals[i].Replace(",", "")),
                        Promotion = long.Parse(promotions[i].Replace(",", "")),
                        RealAmount = 0,
                        RealTotal = 0,
                    });
                }
            }
            if (OrderRepository.UseInstance.Insert(order))
            {
                if (!idProducts.IsNullOrEmpty())
                {
                    productOrders.ForEach(t =>
                    {
                        t.IDOrder = order.ID;
                    });
                    ProductOrder.UseInstance.BulkInserts(productOrders);
                }
                SetSuccess("Tạo mới đơn thành công");
            }
            else
            {
                SetError("Tạo mới đơn không thành công");
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
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = Utils.GetInt(DATA, "Status");
            var productOrders = ProductOrderRepository.UseInstance.GetListByFieldOrDefault("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
            var products = ProductRepository.UseInstance.GetListOrDefault();
            var calUnits = CategoryRepository.UseInstance.GetListByFieldOrDefault("IDCategoryType", "1", (int)TypeObject.Int);
            var customer = CustomerRepository.UseInstance.GetByIdOrDefault(order.IDCustomer);
            var supplier = SupplierRepository.UseInstance.GetByIdOrDefault(order.IDSupplier);
            SetTitle("Cập nhật đơn");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Width = 1000,

                Data = new OrderModel
                {
                    Action = (int)ActionStatus.Update,
                    Order = order,
                    CalUnits = calUnits,
                    ProductOrders = productOrders,
                    Products = products,
                    Status = (int)OrderStatus.Receive,
                    Customer = customer,
                    Supplier = supplier,
                    Url = "/Order/Change"
                }
            });
        }
        public ActionResult Change()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order = order.BindData(DATA, false);
            if (!IsValidate(order))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Updated = DateTime.Now;
            #region old Order detail
            var oldProductOders = ProductOrder.UseInstance.GetListByFieldOrDefault("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
            #endregion

            #region new Order
            var idProducts = Utils.GetInts(DATA, "ProductID", IsAjax);
            var amounts = Utils.GetStrings(DATA, "ProductAmount", IsAjax);
            var promotions = Utils.GetStrings(DATA, "ProductPromotion", IsAjax);
            var totals = Utils.GetStrings(DATA, "Total", IsAjax);
            var newProductOrders = new List<ProductOrder>();
            if (!idProducts.IsNullOrEmpty())
            {
                for (int i = 0; i < idProducts.Length; i++)
                {
                    newProductOrders.Add(new ProductOrder
                    {
                        IDChannel = 0,
                        IDOrder = order.ID,
                        IDProduct = idProducts[i],
                        Amount = int.Parse(amounts[i].Replace(",", "")),
                        Promotion = int.Parse(promotions[i].Replace(",", "")),
                        Total = long.Parse(totals[i].Replace(",", "")),
                        Updated = DateTime.Now,
                    });
                }
            }
            #endregion
            var valueOfOrder = long.Parse(Utils.GetString(DATA, "ValueOfOrder").Replace(",", ""));
            order.RealValue = valueOfOrder;
            order.Value = valueOfOrder;
            order.Type = (int)OrderType.Order;
            if (OrderRepository.UseInstance.Update(order))
            {
                if (!Equals(oldProductOders, null))
                {
                    var idProductOders = oldProductOders.Select(t => (long)t.ID).ToArray();
                    ProductOrder.UseInstance.Deletes(idProductOders);
                }
                if (!idProducts.IsNullOrEmpty())
                {
                    ProductOrder.UseInstance.BulkInserts(newProductOrders);
                }
                SetSuccess("Chỉnh sửa thông tin đơn thành công");
            }
            else
            {
                SetError("Chỉnh sửa thông tin đơn không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsDelete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var Order = OrderRepository.UseInstance.GetById(id);
            if (Equals(Order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    ID = id,
                    //   Class ="",
                    //Target = ".ui-dialog:visible",
                    Title = string.Format("Xóa thông tin đơn"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin đơn [{0}]?", Order.PONo),
                    Url = "/Order/delete"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 400
            });
        }
        public ActionResult Delete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (OrderRepository.UseInstance.Delete(order.ID))
            {
                ProductOrder.UseInstance.DeleteByField("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
                OrderFileRepository.UseInstance.DeleteByField("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
                AccountingDeptRepository.UseInstance.DeleteByField("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
                SetSuccess(string.Format("Xóa thông tin của đơn [{0}] thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Xóa thông tin của đơn [0] không thành công"));
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
                    Title = string.Format("Xóa thông tin nhiều đơn"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin của [{0}] đơn?", ids.Length),
                    IsMulti = true,
                    Url = "/Order/deletes"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 400
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
            if (OrderRepository.UseInstance.Deletes(ids))
            {
                ProductOrderRepository.DeleteByFieldLongIDs("IDOrder", ids);
                OrderFileRepository.UseInstance.DeleteByField("IDOrder", ids);
                AccountingDeptRepository.UseInstance.DeleteByField("IDOrder", ids);
                SetSuccess(string.Format("Xóa [{0}] đơn thành công", ids.Length));
            }
            else
            {
                SetError(string.Format("Xóa [{0}] đơn không thành công", ids.Length));
            }

            return GetResultOrReferrerDefault(defauthPath);
            // var
        }
        public ActionResult Detail()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            var tab = Utils.GetString(DATA, "tab");
            tab = tab.IsNullOrEmpty() ? "device_OverViews" : tab;
            var searchParam = new SearchParam().BindData(DATA);
            var isAD = Utils.GetBool(DATA, "IsAD");
            var productOrders = ProductOrderRepository.UseInstance.GetListByFieldOrDefault("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
            var products = ProductRepository.UseInstance.GetListOrDefault();
            var calUnits = CategoryRepository.UseInstance.GetListByFieldOrDefault("IDCategoryType", "1", (int)TypeObject.Int);
            var customer = CustomerRepository.UseInstance.GetByIdOrDefault(order.IDCustomer);
            var supplier = SupplierRepository.UseInstance.GetByIdOrDefault(order.IDSupplier);
            var olderFiles = OrderFile.UseInstance.GetListByFieldOrDefault("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
            SetTitle("Chi tiêt đơn hàng");
            return GetCustResultOrView(new ViewParam
            {
                Data = new OrderModel
                {
                    ProductOrders = productOrders,
                    Products = products,
                    CalUnits = calUnits,
                    Customer = customer,
                    Supplier = supplier,
                    OrderFiles = olderFiles,
                    Order = order,
                    Tab = tab,
                    IsAD = isAD,
                    SearchParam = searchParam,
                },
                ViewName = "Detail",
                ViewNameAjax = "Detail"
            });
        }
        #endregion
        #region Delivery
        public ActionResult IsDelivery()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = Utils.GetInt(DATA, "Status");
            var productOrders = ProductOrderRepository.UseInstance.GetListByFieldOrDefault("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
            var products = ProductRepository.UseInstance.GetListOrDefault();
            var calUnits = CategoryRepository.UseInstance.GetListByFieldOrDefault("IDCategoryType", "1", (int)TypeObject.Int);
            var customer = CustomerRepository.UseInstance.GetByIdOrDefault(order.IDCustomer);
            var supplier = SupplierRepository.UseInstance.GetByIdOrDefault(order.IDSupplier);
            SetTitle("Xác nhận giao hàng");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Width = 1000,
                Data = new OrderModel
                {
                    Action = (int)ActionStatus.View,
                    Order = order,
                    CalUnits = calUnits,
                    ProductOrders = productOrders,
                    Products = products,
                    Status = (int)OrderStatus.Delivery,
                    Customer = customer,
                    Supplier = supplier,
                    Url = "/Order/DeliveryChange"
                }
            }); ;
        }
        public ActionResult DeliveryChange()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order = order.BindData(DATA, false);
            order.RealDeliveryDate = DateTime.Now;
            if (!IsValidate(order))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = (int)OrderStatus.Delivery;
            #region old Order detail
            var oldProductOders = ProductOrder.UseInstance.GetListByFieldOrDefault("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
            #endregion

            #region Update Order
            var amounts = Utils.GetStrings(DATA, "ProductAmount", IsAjax);
            var totals = Utils.GetStrings(DATA, "Total", IsAjax);
            var promotions = Utils.GetStrings(DATA, "ProductPromotion", IsAjax);
            if (!Equals(oldProductOders, null))
            {
                for (int i = 0; i < oldProductOders.Count; i++)
                {
                    oldProductOders[i].RealAmount = int.Parse(amounts[i].Replace(",", ""));
                    oldProductOders[i].RealTotal = int.Parse(totals[i].Replace(",", ""));
                    oldProductOders[i].RealPromotion = int.Parse(promotions[i].Replace(",", ""));
                }
            }
            #endregion
            var valueOfOrder = Utils.GetString(DATA, "ValueOfOrder").Replace(",", "");
            order.RealValue = long.Parse(valueOfOrder);
            if (OrderRepository.UseInstance.Update(order))
            {
                if (!Equals(oldProductOders, null) && oldProductOders.Count > 0)
                {
                    oldProductOders.ForEach(t =>
                    {
                        ProductOrderRepository.UseInstance.Update(t);
                    });
                }
                SetSuccess("Xác nhận giào hàng thành công");
            }
            else
            {
                SetError("Xác nhận giào hàng không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        #endregion

        #region IRVReturn
        public ActionResult IsIRVReturn()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            var orderFile = OrderFileRepository.GetFirstOrDefault(new OrderFileParam
            {
                IDOrder = order.ID,
                Types = new int[] { (int)OrderFileType.IRV }
            });
            ViewBag.IRVContent = string.Format("Tải file xác nhận trả phiếu nhập kho");
            SetTitle("Xác nhận trả phiếu nhập kho");
            return GetDialogResultOrView(new ViewParam
            {
                Data = new OrderModel
                {
                    Order = order,
                    OrderFile = orderFile,
                    Url = "/Order/IRVReturn",
                },
                ViewName = "IRVReturn",
                ViewNameAjax = "IRVReturn",
                Width = 520
            });
        }
        public ActionResult IRVReturn()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = (int)OrderStatus.IRVReturn;
            order.IRVReturnDate = DateTime.Now;
            var paths = Utils.GetStrings(DATA, "Path", IsAjax);
            var fileNames = Utils.GetStrings(DATA, "FileName", IsAjax);
            var attchFiles = new List<OrderFile>();
            if (Equals(paths, null) || paths.Length < 1)
            {
                SetError("Bạn chưa chọn chọn file nào");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (!paths.IsNullOrEmpty())
            {
                for (int i = 0; i < paths.Length; i++)
                {
                    attchFiles.Add(new OrderFile
                    {
                        IDChannel = 0,
                        IDOrder = order.ID,
                        FileName = fileNames[i],
                        Path = paths[i],
                        OrderStatus = order.Status,
                        Type = (int)OrderFileType.IRV
                    });
                }
            }
            if (OrderRepository.UseInstance.Update(order))
            {
                OrderFileRepository.UseInstance.DeleteByField(new List<CondParam>{
                    new CondParam
                    {
                        Field = "IDOrder", Value = order.ID, CompareType = (int)CompareTypes.Equal, TypeSQL = (int)TypeSQl.Number
                    },
                      new CondParam
                    {
                        Field = "OrderStatus", Value = (int)OrderStatus.IRVReturn, CompareType = (int)CompareTypes.Equal, TypeSQL = (int)TypeSQl.Number
                    }
                });
                OrderFileRepository.UseInstance.BulkInserts(attchFiles);
                SetSuccess(string.Format("Xác nhận trả phiếu nhập kho của đơn [{0}] thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Xác nhận trả phiếu nhập kho của đơn [{0}] không thành công", order.PONo));
            }

            return GetResultOrReferrerDefault(defauthPath);
        }
        #endregion

        #region CollectVouchers
        public ActionResult IsCollectVouchers()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            var orderFiles = OrderFileRepository.UseInstance.GetListByFieldsOrDefault(new List<CondParam> {
                new CondParam
                {
                    Field = "IDOrder", Value =order.ID,CompareType= (int)CompareTypes.Equal, TypeSQL = (int)TypeSQl.String
                },
                 new CondParam
                {
                    Field = "OrderStatus", Value =(int)OrderStatus.CollectVouchers,CompareType= (int)CompareTypes.Equal, TypeSQL = (int)TypeSQl.String
                }
            });
            ViewBag.CollectionContent = string.Format("Tải file xác nhận trả thu thập chứng từ");
            SetTitle("Xác nhận thu thập chứng từ");
            return GetDialogResultOrView(new ViewParam
            {
                Data = new OrderModel
                {
                    Order = order,
                    OrderFiles = orderFiles,
                    Url = string.Format("/Order/CollectVouchers"),
                },
                ViewName = "CollectVouchers",
                ViewNameAjax = "CollectVouchers",
                Width = 520
            });
        }
        public ActionResult CollectVouchers()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = (int)OrderStatus.CollectVouchers;
            order.CollectedDate = DateTime.Now;
            var paths = Utils.GetStrings(DATA, "Path", IsAjax);
            var fileNames = Utils.GetStrings(DATA, "FileName", IsAjax);
            var attchFiles = new List<OrderFile>();
            var types = Utils.GetInts(DATA, "Type", IsAjax);
            if (Equals(paths, null) || paths.Length < 1)
            {
                SetError("Bạn chưa chọn chọn file nào");
                return GetResultOrReferrerDefault(defauthPath);
            }
            else if (paths.Length < 3)
            {
                SetError("Bạn chưa chọn chọn đủ 3 file");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (!paths.IsNullOrEmpty())
            {
                for (int i = 0; i < paths.Length; i++)
                {
                    attchFiles.Add(new OrderFile
                    {
                        IDChannel = 0,
                        IDOrder = order.ID,
                        FileName = fileNames[i],
                        OrderStatus = order.Status,
                        Path = paths[i],
                        Type = types[i]
                    });
                }
            }
            if (OrderRepository.UseInstance.Update(order))
            {
                OrderFileRepository.UseInstance.DeleteByField(new List<CondParam>{
                    new CondParam
                    {
                        Field = "IDOrder", Value = order.ID, CompareType = (int)CompareTypes.Equal, TypeSQL = (int)TypeSQl.Number
                    },
                      new CondParam
                    {
                        Field = "OrderStatus", Value = (int)OrderStatus.CollectVouchers, CompareType = (int)CompareTypes.Equal, TypeSQL = (int)TypeSQl.Number
                    }
                });
                OrderFileRepository.UseInstance.BulkInserts(attchFiles);
                SetSuccess(string.Format("Xác nhận thu thập chứng từ của đơn [{0}] thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Xác nhận thu thập chứng từ của đơn [{0}] không thành công", order.PONo));
            }

            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsCollectVouchersUpdate()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = Utils.GetInt(DATA, "Status");
            var productOrders = ProductOrderRepository.UseInstance.GetListByFieldOrDefault("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
            var products = ProductRepository.UseInstance.GetListOrDefault();
            var calUnits = CategoryRepository.UseInstance.GetListByFieldOrDefault("IDCategoryType", "1", (int)TypeObject.Int);
            var customer = CustomerRepository.UseInstance.GetByIdOrDefault(order.IDCustomer);
            var supplier = SupplierRepository.UseInstance.GetByIdOrDefault(order.IDSupplier);
            SetTitle("Cập nhật đơn hàng phòng kế toán trả lại");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Width = 1000,
                Data = new OrderModel
                {
                    Action = (int)ActionStatus.Update,
                    Order = order,
                    CalUnits = calUnits,
                    ProductOrders = productOrders,
                    Products = products,
                    Status = (int)OrderStatus.CollectVouchers,
                    Customer = customer,
                    Supplier = supplier,
                    Url = "/Order/DeliveryChange"
                }
            }); ;
        }
        public ActionResult CollectVouchersChange()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order = order.BindData(DATA, false);
            if (!IsValidate(order))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = (int)OrderStatus.CollectVouchers;
            #region old Order detail
            var oldProductOders = ProductOrder.UseInstance.GetListByFieldOrDefault("IDOrder", order.ID.ToString(), (int)TypeObject.Int);
            #endregion

            #region Update Order
            var amounts = Utils.GetStrings(DATA, "ProductAmount", IsAjax);
            var totals = Utils.GetStrings(DATA, "Total", IsAjax);
            var promotions = Utils.GetStrings(DATA, "ProductPromotion", IsAjax);
            if (!Equals(oldProductOders, null))
            {
                for (int i = 0; i < oldProductOders.Count; i++)
                {
                    oldProductOders[i].RealAmount = int.Parse(amounts[i].Replace(",", ""));
                    oldProductOders[i].RealTotal = int.Parse(totals[i].Replace(",", ""));
                    oldProductOders[i].RealPromotion = int.Parse(promotions[i].Replace(",", ""));
                }
            }
            #endregion
            var valueOfOrder = Utils.GetString(DATA, "ValueOfOrder").Replace(",", "");
            order.RealValue = long.Parse(valueOfOrder);
            if (OrderRepository.UseInstance.Update(order))
            {
                if (!Equals(oldProductOders, null) && oldProductOders.Count > 0)
                {
                    oldProductOders.ForEach(t =>
                    {
                        ProductOrderRepository.UseInstance.Update(t);
                    });
                }
                SetSuccess("Cập nhật thông tin hóa đơn thành công");
            }
            else
            {
                SetError("Cập nhật thông tin hóa đơn không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        #endregion

        #region Payed
        public ActionResult IsPayed()
        {
            var id = Utils.GetInt(DATA, "ID");
            var Order = OrderRepository.UseInstance.GetById(id);
            if (Equals(Order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    ID = id,
                    Title = string.Format("Xác nhận thanh toán hóa đơn"),
                    Content = string.Format("Bạn có xác nhận thanh toán hóa đơn cho đơn [{0}]?", Order.PONo),
                    Url = "/Order/Payed",
                    Class = "btn-success",
                },
                ViewName = "~/Views/Shared/IsConfirm.cshtml",
                ViewNameAjax = "~/Views/Shared/IsConfirm.cshtml",
                Width = 500
            });
        }
        public ActionResult Payed()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = (int)OrderStatus.Payed;
            order.PayedDate = DateTime.Now;
            if (OrderRepository.UseInstance.Update(order))
            {
                SetSuccess(string.Format("Xác nhận chuyển đến phòng kế toán của đơn [{0}] thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Xác nhận thu thập chứng từ của đơn [{0}] không thành công", order.PONo));
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsPayedConfirm()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            ViewBag.Content = string.Format("Bạn có chắc muốn xác nhận thanh toán cho hóa đơn [{0}] không?", order.PONo);
            SetTitle("Xác nhận địa điểm thanh toán");
            return GetDialogResultOrView(new ViewParam
            {
                Data = new OrderModel
                {
                    Order = order,
                    Url = "/Order/PayedConfirm",
                },
                ViewName = "PayedConfirm",
                ViewNameAjax = "PayedConfirm",
                Width = 520
            });
        }
        public ActionResult PayedConfirm()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.Status = (int)OrderStatus.PayedConfirm;
            order.PayedConfirmDate = DateTime.Now;
            order.PayedPlace = Utils.GetString(DATA, "PayedPlace");
            if (OrderRepository.UseInstance.Update(order))
            {
                SetSuccess(string.Format("Xác nhận thanh toán của đơn [{0}] thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Xác nhận thanh toán của đơn [{0}] không thành công", order.PONo));
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsPayedReplace()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            ViewBag.PayedCss = "btn-primary";
            SetTitle("Cập nhật địa điểm thanh toán");
            return GetDialogResultOrView(new ViewParam
            {
                Data = new OrderModel
                {
                    Order = order,
                    Url = "/Order/PayedConfirm",
                },
                ViewName = "PayedConfirm",
                ViewNameAjax = "PayedConfirm",
                Width = 520
            });
        }
        public ActionResult PayedReplace()
        {
            var id = Utils.GetInt(DATA, "ID");
            var order = OrderRepository.UseInstance.GetById(id);
            if (Equals(order, null))
            {
                SetError("Thông tin đơn không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            order.PayedPlace = Utils.GetString(DATA, "PayedPlace");
            if (OrderRepository.UseInstance.Update(order))
            {
                SetSuccess(string.Format("Cập nhật địa điểm thanh toán thành công", order.PONo));
            }
            else
            {
                SetError(string.Format("Cập nhật địa điểm thanh toán không thành công", order.PONo));
            }

            return GetResultOrReferrerDefault(defauthPath);
        }
        #endregion
        #region Genaral Function
        private bool IsValidate(Order order)
        {
            var compareDate = (order.PODate - order.DeliveryDate).Value.Ticks;
            if (OrderRepository.UseInstance.FieldExist("PONo", order.PONo, order.ID))
            {
                SetError("Đơn hàng đã tồn tại");
            }
            else if (compareDate > 0)
            {
                SetError("Thời gian đặt hàng không được lớn hơn thời gian giao hàng");
            }
            else if (order.IDCustomer <= 0)
            {
                SetError("Bạn chưa chọn khách hàng nào");
            }
            else if (order.IDSupplier <= 0)
            {
                SetError("Bạn chưa chọn nhà cung cấp nào");
            }
            return !HasError;
        }
        public ActionResult GetProduct()
        {
            var id = Utils.GetInt(DATA, "id");
            var temp = Utils.GetString(DATA, "temp");
            var trTarget = Utils.GetString(DATA, "trTarget");
            var product = ProductRepository.UseInstance.GetByIdOrDefault(id);
            var calUnit = CategoryRepository.UseInstance.GetByIdOrDefault(product.CalUnit);
            return Json(new
            {
                isCustom = true,
                Temp = temp,
                trTarget = trTarget,
                data = new
                {
                    ProductID = product.ID,
                    ProductName = product.Name,
                    ProductBarCode = product.BarCode,
                    ProductCalUnitName = calUnit.Name,
                    ProductPrice = product.Price > 0 ? string.Format("{0,00}", product.Price) : "0",
                    ProductAmount = "0",
                    Total = "0",
                },
            });
        }

        #endregion
    }
}