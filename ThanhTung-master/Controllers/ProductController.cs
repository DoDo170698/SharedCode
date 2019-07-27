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
    public class ProductController : BaseController
    {
        string defauthPath = "/Product";
        public ActionResult Index()
        {
            var searchParm = new SearchParam().BindData(DATA);
            var products = ProductRepository.Search(searchParm, Paging);
            var categoryType = CategoryTypeRepository.UseInstance.GetSingleByField("Code", "dm_hs", (int)TypeObject.String) ?? new CategoryType();
            var productTypes = CategoryRepository.UseInstance.GetListByFieldOrDefault("IDCategoryType", categoryType.ID.ToString(), (int)TypeObject.Int);
            SetTitle("Danh sách hàng hóa");
            return GetCustResultOrView(new ViewParam
            {
                Data = new ProductModel
                {
                    Products = products,
                    ProductTypes = productTypes,
                    SearchParam = searchParm,
                },
                ViewName = "Index",
                ViewNameAjax = "Products"
            });
        }
        public ActionResult Create()
        {
            SetTitle("Tạo mới hàng hóa");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Create",
                ViewNameAjax = "Create",
                Data = new ProductModel
                {
                    ProductType = new Category(),
                    Product = new Product(),
                    CalUnit = new Category(),
                    Url = "/Product/save"
                }
            }); ;
        }
        public ActionResult Save()
        {
            var product = new Product().BindData(DATA);
            if (!IsValidate(product))
            {
                return GetResult();
            }
            var value = Utils.GetString(DATA, "Price").Replace(",", string.Empty);
            product.Price = long.Parse(value);
            if (ProductRepository.UseInstance.Insert(product))
            {
                SetSuccess("Tạo mới hàng hóa thành công");
            }
            else
            {
                SetError("Tạo mới hàng hóa không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult Update()
        {
            var id = Utils.GetInt(DATA, "ID");
            var product = ProductRepository.UseInstance.GetById(id);
            if (Equals(product, null))
            {
                SetError("Thông tin hàng hóa không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            var producType = CategoryRepository.UseInstance.GetByIdOrDefault(product.Type);
            var calUnit = CategoryRepository.UseInstance.GetByIdOrDefault(product.CalUnit);
            SetTitle("Cập nhật hàng hóa");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Data = new ProductModel
                {
                    ProductType = producType,
                    CalUnit = calUnit,
                    Product = product,
                    Url = "/Product/Change"
                }
            }); ;
        }
        public ActionResult Change()
        {
            var id = Utils.GetInt(DATA, "ID");
            var product = ProductRepository.UseInstance.GetById(id);
            if (!IsValidate(product))
            {
                return GetResultOrReferrerDefault(defauthPath);
            }
            product.BindData(DATA,false);
            var value = Utils.GetString(DATA, "Price").Replace(",", string.Empty);
            product.Price = long.Parse(value);
            if (ProductRepository.UseInstance.Update(product))
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
            var Product = ProductRepository.UseInstance.GetById(id);
            if (Equals(Product, null))
            {
                SetError("Thông tin hàng hóa không còn tồn tại");
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
                    Content = string.Format("Bạn có chắc muốn xóa thông tin hàng hóa [{0}]?", Product.Name),
                    Url = "/Product/delete"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 400
            });
        }
        public ActionResult Delete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var Product = ProductRepository.UseInstance.GetById(id);
            if (Equals(Product, null))
            {
                SetError("Thông tin hàng hóa không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (ProductRepository.UseInstance.Delete(Product.ID))
            {
                SetSuccess(string.Format("Xóa thông tin của hàng hóa [{0}] thành công", Product.Name));
            }
            else
            {
                SetError(string.Format("Xóa thông tin của hàng hóa [0] không thành công"));
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
                    Title = string.Format("Xóa thông tin nhiều hàng hóa"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin của [{0}] hàng hóa?", ids.Length),
                    IsMulti = true,
                    Url = "/Product/deletes"
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
            if (ProductRepository.UseInstance.Deletes(ids))
            {
                SetSuccess(string.Format("Xóa [{0}] hàng hóa thành công", ids.Length));
            }
            else
            {
                SetError(string.Format("Xóa [{0}] hàng hóa không thành công", ids.Length));
            }

            return GetResultOrReferrerDefault(defauthPath);
            // var
        }
        private bool IsValidate(Product Product)
        {
            if (ProductRepository.UseInstance.FieldExist("Name", Product.Name, Product.ID))
            {
                SetError("Tên hàng hóa đã tồn tại");
            }else if (Product.Type <= 0)
            {
                SetError("Loại hàng hóa không dược để trống");
            }

            return !HasError;
        }
    }
}