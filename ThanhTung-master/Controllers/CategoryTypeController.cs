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

namespace QuanLyHoaDon.Controllers
{
    public class CategoryTypeController : BaseController
    {
        string defauthPath = "/CategoryType";
        public ActionResult Index()
        {
            var searchParm = new SearchParam().BindData(DATA);
            var categoryTypes = CategoryTypeRepository.Search(searchParm, Paging);
            SetTitle("Danh sách loại danh mục");
            return GetCustResultOrView(new ViewParam
            {
                Data = new AdminModel
                {
                    CategoryTypes = categoryTypes,
                    SearchParam = searchParm,
                },
                ViewName ="Index",
                ViewNameAjax = "CategoryTypes"
            }) ;
        }
        public ActionResult Create()
        {
            SetTitle("Tạo mới loại danh mục");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Create",
                ViewNameAjax = "Create",
                Data = new AdminModel
                {
                    CategoryType = new CategoryType(),
                    Url = "/CategoryType/save"
                }
            }); ;
        }
        public ActionResult Save()
        {
            var categoryType = new CategoryType().BindData(DATA);
            if (!IsValidate(categoryType))
            {
                return GetResult();
            }else if(CategoryTypeRepository.UseInstance.Insert(categoryType))
            {
                SetSuccess("Tạo mới loại danh mục thành công");
            }
            else
            {
                SetError("Tạo mới loại danh mục không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult Update()
        {
            var id = Utils.GetInt(DATA,"ID");
            var CategoryType = CategoryTypeRepository.UseInstance.GetById(id);
            if (Equals(CategoryType,null))
            {
                SetError("Thông tin loại danh mục không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }

            SetTitle("Cập nhật loại danh mục");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Data = new AdminModel
                {
                    CategoryType = CategoryType,
                    Url = "/CategoryType/Change"
                }
            }); ;
        }
        public ActionResult Change()
        {
            var CategoryType = new CategoryType().BindData(DATA,false);
            if (!IsValidate(CategoryType))
            {
                return GetResult();
            }
            else if (CategoryTypeRepository.UseInstance.Update(CategoryType))
            {
                SetSuccess("Chỉnh sửa thông tin loại danh mục thành công");
            }
            else
            {
                SetError("Chỉnh sửa thông tin loại danh mục không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsDelete()
        {
            var id = Utils.GetInt(DATA,"ID");
            var CategoryType = CategoryTypeRepository.UseInstance.GetById(id);
            if (Equals(CategoryType, null))
            {
                SetError("Thông tin loại danh mục không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam {
                Data = new BaseModel
                {
                    ID = id,
                 //   Class ="",
                    //Target = ".ui-dialog:visible",
                    Title =string.Format("Xóa thông tin loại danh mục"),
                    Content =string.Format("Bạn có chắc muốn xóa thông tin loại danh mục [{0}]?",CategoryType.Name),
                    Url = "/CategoryType/delete"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 400
            });
        }
        public ActionResult Delete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var CategoryType = CategoryTypeRepository.UseInstance.GetById(id);
            if (Equals(CategoryType, null))
            {
                SetError("Thông tin loại danh mục không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (CategoryTypeRepository.UseInstance.Delete(CategoryType.ID))
            {
                SetSuccess(string.Format("Xóa thông tin của loại danh mục [{0}] thành công", CategoryType.Name));
            }
            else
            {
                SetError(string.Format("Xóa thông tin của loại danh mục [0] không thành công"));
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
                    Title = string.Format("Xóa thông tin nhiều loại danh mục"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin của [{0}] loại danh mục?",ids.Length),
                    IsMulti =true,
                    Url = "/CategoryType/deletes"
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
            if (CategoryTypeRepository.UseInstance.Deletes(ids))
            {
                SetSuccess(string.Format("Xóa [{0}] loại danh mục thành công",ids.Length));
            }
            else
            {
                SetError(string.Format("Xóa [{0}] loại danh mục không thành công",ids.Length));
            }

            return GetResultOrReferrerDefault(defauthPath);
            // var
        }
        private bool IsValidate(CategoryType CategoryType)
        {
            if (CategoryTypeRepository.UseInstance.FieldExist("Name",CategoryType.Name,CategoryType.ID))
            {
                SetError("Tên loại danh mục đã tồn tại");
            }
            else if(CategoryTypeRepository.UseInstance.FieldExist("Code", CategoryType.Name, CategoryType.ID))
            {
                SetError("Mã loại danh mục đã tồn tại");
            }

            return !HasError;
        }

    }
}