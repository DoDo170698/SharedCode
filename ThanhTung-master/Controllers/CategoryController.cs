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
    public class CategoryController : BaseController
    {
        string defauthPath = "/Category";
        public ActionResult Index()
        {
            // commit ngày 18/06/2019

            var searchParm = new SearchParam().BindData(DATA);
            searchParm.Parent = Utils.GetInt(DATA,"ID");
            var categories = CategoryRepository.Search(searchParm, Paging);
            var categoryType = CategoryTypeRepository.UseInstance.GetByIdOrDefault(searchParm.Type);
            var categoryParent = CategoryRepository.UseInstance.GetByIdOrDefault(searchParm.Parent);
            var idAncestors = Utils.GetLongParents(categoryParent.Parents, categoryParent.ID);
            var categoryParents = CategoryRepository.UseInstance.GetByIdsOrDefault(idAncestors.ToArray());
            SetTitle("Danh sách danh mục");
            return GetCustResultOrView(new ViewParam
            {
                Data = new AdminModel
                {
                    Category = new Category { IDCategoryType = categoryType.ID, Parent = searchParm.Parent },
                    Categories = categories,
                    CategoryType = categoryType,
                    Ancestors = categoryParents,
                    Parent = categoryParent,
                    SearchParam = searchParm,
                },
                ViewName = "Index",
                ViewNameAjax = "Categories"
            });
        }
        public ActionResult Create()
        {
            var type = Utils.GetInt(DATA, "Type");
            var categoryType = CategoryTypeRepository.UseInstance.GetById(type);
            if (Equals(categoryType, null))
            {
                SetError("Thông tin loại danh mục không còn tồn tại");
                return GetResultOrReferrerDefault("/CategoryType");
            }
            var parent = Utils.GetInt(DATA, "Parent");
            SetTitle("Tạo mới danh mục");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Create",
                ViewNameAjax = "Create",
                Data = new AdminModel
                {
                    Category = new Category { IDCategoryType = categoryType.ID, Parent = parent },
                    Url = "/Category/save"
                }
            }); ;
        }
        public ActionResult Save()
        {
            var category = new Category().BindData(DATA);
            if (!IsValidate(category))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            var categoryParent = CategoryRepository.UseInstance.GetByIdOrDefault(category.Parent);
            category.Parents = Utils.GetStringParents(categoryParent.Parents, categoryParent.ID);
            if (CategoryRepository.UseInstance.Insert(category))
            {
                SetSuccess("Tạo mới danh mục thành công");
            }
            else
            {
                SetError("Tạo mới danh mục không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult Update()
        {
            var id = Utils.GetInt(DATA, "ID");
            var category = CategoryRepository.UseInstance.GetById(id);
            if (Equals(category, null))
            {
                SetError("Thông tin danh mục không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            SetTitle("Cập nhật danh mục");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Data = new AdminModel
                {
                    Category = category,
                    Url = "/Category/Change"
                }
            }); ;
        }
        public ActionResult Change()
        {
            var id = Utils.GetInt(DATA, "ID");
            var category = CategoryRepository.UseInstance.GetById(id);
            if (!IsValidate(category))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            category = category.BindData(DATA, false);
            if (CategoryRepository.UseInstance.Update(category))
            {
                SetSuccess("Chỉnh sửa thông tin danh mục thành công");
            }
            else
            {
                SetError("Chỉnh sửa thông tin danh mục không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult IsDelete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var category = CategoryRepository.UseInstance.GetById(id);
            if (Equals(category, null))
            {
                SetError("Thông tin danh mục không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam
            {
                Data = new BaseModel
                {
                    ID = id,
                    Title = string.Format("Xóa thông tin danh mục"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin danh mục [{0}]?", category.Name),
                    Url = "/Category/delete"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 400
            });
        }
        public ActionResult Delete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var category = CategoryRepository.UseInstance.GetById(id);
            if (Equals(category, null))
            {
                SetError("Thông tin danh mục không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (CategoryRepository.UseInstance.Delete(category.ID))
            {
                CategoryRepository.DelChildNotParent();
                SetSuccess(string.Format("Xóa thông tin của danh mục [{0}] thành công", category.Name));
            }
            else
            {
                SetError(string.Format("Xóa thông tin của danh mục [0] không thành công"));
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
                    Title = string.Format("Xóa thông tin nhiều danh mục"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin của [{0}] danh mục?", ids.Length),
                    IsMulti = true,
                    Url = "/Category/deletes"
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
            if (CategoryRepository.UseInstance.Deletes(ids))
            {
                CategoryRepository.DelChildNotParent();
                SetSuccess(string.Format("Xóa [{0}] danh mục thành công", ids.Length));
            }
            else
            {
                SetError(string.Format("Xóa [{0}] danh mục không thành công", ids.Length));
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        private bool IsValidate(Category category)
        {
            if (CategoryRepository.NameExist(category.IDCategoryType, category.Parent, category.Name, category.ID))
            {
                SetError("Tên danh mục đã tồn tại");
            }
            return !HasError;
        }

    }
}