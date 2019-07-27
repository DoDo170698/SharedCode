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

namespace QuanLyHoaDon.Controllers
{
    public class SupplierController : BaseController
    {
        string defauthPath = "/Supplier";
        public ActionResult Index()
        {
            var searchParm = new SearchParam().BindData(DATA);
            var suppliers = SupplierRepository.Search(searchParm, Paging);
            SetTitle("Danh sách nhà cung cấp");
            return GetCustResultOrView(new ViewParam
            {
                Data = new SupplierModel
                {
                    Suppliers = suppliers,
                    SearchParam = searchParm,
                },
                ViewName ="Index",
                ViewNameAjax = "Suppliers"
            }) ;
        }
        public ActionResult Create()
        {
            SetTitle("Tạo mới nhà cung cấp");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Create",
                ViewNameAjax = "Create",
                Data = new SupplierModel
                {
                    Supplier = new Supplier(),
                    Url = "/Supplier/save"
                }
            }); ;
        }
        public ActionResult Save()
        {
            var supplier = new Supplier().BindData(DATA);
            if (!IsValidate(supplier))
            {
                return GetResult();
            }else if(SupplierRepository.UseInstance.Insert(supplier))
            {
                SetSuccess("Tạo mới nhà cung cấp thành công");
            }
            else
            {
                SetError("Tạo mới nhà cung cấp không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult Update()
        {
            var id = Utils.GetInt(DATA,"ID");
            var supplier = SupplierRepository.UseInstance.GetById(id);
            if (Equals(supplier,null))
            {
                SetError("Thông tin nhà cung cấp không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            SetTitle("Cập nhật nhà cung cấp");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Data = new SupplierModel
                {
                    Supplier = supplier,
                    Url = "/Supplier/Change"
                }
            }); ;
        }
        public ActionResult Change()
        {
            var supplier = new Supplier().BindData(DATA,false);
            if (!IsValidate(supplier))
            {
                return GetResult();
            }
            else if (SupplierRepository.UseInstance.Update(supplier))
            {
                SetSuccess("Chỉnh sửa thông tin nhà cung cấp thành công");
            }
            else
            {
                SetError("Chỉnh sửa thông tin nhà cung cấp không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsDelete()
        {
            var id = Utils.GetInt(DATA,"ID");
            var supplier = SupplierRepository.UseInstance.GetById(id);
            if (Equals(supplier, null))
            {
                SetError("Thông tin nhà cung cấp không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam {
                Data = new BaseModel
                {
                    ID = id,
                 //   Class ="",
                    //Target = ".ui-dialog:visible",
                    Title =string.Format("Xóa thông tin nhà cung cấp"),
                    Content =string.Format("Bạn có chắc muốn xóa thông tin nhà cung cấp [{0}]?",supplier.Name),
                    Url = "/Supplier/delete"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 400
            });
        }
        public ActionResult Delete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var Supplier = SupplierRepository.UseInstance.GetById(id);
            if (Equals(Supplier, null))
            {
                SetError("Thông tin nhà cung cấp không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (SupplierRepository.UseInstance.Delete(Supplier.ID))
            {
                SetSuccess(string.Format("Xóa thông tin của nhà cung cấp [{0}] thành công", Supplier.Name));
            }
            else
            {
                SetError(string.Format("Xóa thông tin của nhà cung cấp [0] không thành công"));
            }

            return GetResultOrReferrerDefault(defauthPath);
        }

        public ActionResult IsDeletes()
        {

            var ids = Utils.GetString(DATA,"ID[]").Split(',');
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
                    Title = string.Format("Xóa thông tin nhiều nhà cung cấp"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin của [{0}] nhà cung cấp?",ids.Length),
                    IsMulti =true,
                    Url = "/Supplier/deletes"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 400
            });
        }
        public ActionResult Deletes()
        {
            var ids = Utils.GetString(DATA,"IDs").DeSerialize<long[]>();
            if (!ids.Any())
            {
                SetError("Bạn chưa chọn thông tin nào để xóa");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (SupplierRepository.UseInstance.Deletes(ids))
            {
                SetSuccess(string.Format("Xóa [{0}] nhà cung cấp thành công",ids.Length));
            }
            else
            {
                SetError(string.Format("Xóa [{0}] nhà cung cấp không thành công",ids.Length));
            }

            return GetResultOrReferrerDefault(defauthPath);
            // var
        }

        private bool IsValidate(Supplier Supplier)
        {
            if (SupplierRepository.UseInstance.FieldExist("Name",Supplier.Name,Supplier.ID))
            {
                SetError("Tên nhà cung cấp đã tồn tại");
            }

            return !HasError;
        }

    }
}