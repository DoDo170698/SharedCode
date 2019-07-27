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
    public class CustomerController : BaseController
    {
        string defauthPath = "/customer";
        public ActionResult Index()
        {
            var searchParm = new SearchParam().BindData(DATA);
            var customers = CustomerRepository.Search(searchParm, Paging);
            SetTitle("Danh sách khách hàng");
            return GetCustResultOrView(new ViewParam
            {
                Data = new CustomerModel
                {
                    Customers = customers,
                    SearchParam = searchParm,
                },
                ViewName ="Index",
                ViewNameAjax = "Customers"
            }) ;
        }
        public ActionResult Create()
        {
            SetTitle("Tạo mới khách hàng");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Create",
                ViewNameAjax = "Create",
                Data = new CustomerModel
                {
                    Customer = new Customer(),
                    Url = "/customer/save"
                }
            }); ;
        }
        public ActionResult Save()
        {
            var customer = new Customer().BindData(DATA);
            if (!IsValidate(customer))
            {
                return GetResult();
            }else if(CustomerRepository.UseInstance.Insert(customer))
            {
                SetSuccess("Tạo mới khách hàng thành công");
            }
            else
            {
                SetError("Tạo mới khách hàng không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }
        public ActionResult Update()
        {
            var id = Utils.GetInt(DATA,"ID");
            var customer = CustomerRepository.UseInstance.GetById(id);
            if (Equals(customer,null))
            {
                SetError("Thông tin khách hàng không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }

            SetTitle("Cập nhật khách hàng");
            return GetDialogResultOrView(new ViewParam
            {
                ViewName = "Update",
                ViewNameAjax = "Update",
                Data = new CustomerModel
                {
                    Customer = customer,
                    Url = "/customer/Change"
                }
            }); ;
        }
        public ActionResult Change()
        {
            var customer = new Customer().BindData(DATA,false);
            if (!IsValidate(customer))
            {
                return GetResult();
            }
            else if (CustomerRepository.UseInstance.Update(customer))
            {
                SetSuccess("Chỉnh sửa thông tin khách hàng thành công");
            }
            else
            {
                SetError("Chỉnh sửa thông tin khách hàng không thành công");
            }
            return GetResultOrReferrerDefault(defauthPath);
        }

        public ActionResult IsDelete()
        {
            var id = Utils.GetInt(DATA,"ID");
            var customer = CustomerRepository.UseInstance.GetById(id);
            if (Equals(customer, null))
            {
                SetError("Thông tin khách hàng không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            return GetDialogResultOrView(new ViewParam {
                Data = new BaseModel
                {
                    ID = id,
                 //   Class ="",
                    //Target = ".ui-dialog:visible",
                    Title =string.Format("Xóa thông tin khách hàng"),
                    Content =string.Format("Bạn có chắc muốn xóa thông tin khách hàng [{0}]?",customer.Name),
                    Url = "/customer/delete"
                },
                ViewName = "~/Views/Shared/IsDelete.cshtml",
                ViewNameAjax = "~/Views/Shared/IsDelete.cshtml",
                Width = 400
            });
        }
        public ActionResult Delete()
        {
            var id = Utils.GetInt(DATA, "ID");
            var customer = CustomerRepository.UseInstance.GetById(id);
            if (Equals(customer, null))
            {
                SetError("Thông tin khách hàng không còn tồn tại");
                return GetResultOrReferrerDefault(defauthPath);
            }
            if (CustomerRepository.UseInstance.Delete(customer.ID))
            {
                SetSuccess(string.Format("Xóa thông tin của khách hàng [{0}] thành công", customer.Name));
            }
            else
            {
                SetError(string.Format("Xóa thông tin của khách hàng [0] không thành công"));
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
                    Title = string.Format("Xóa thông tin nhiều khách hàng"),
                    Content = string.Format("Bạn có chắc muốn xóa thông tin của [{0}] khách hàng?",ids.Length),
                    IsMulti =true,
                    Url = "/customer/deletes"
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
            if (CustomerRepository.UseInstance.Deletes(ids))
            {
                SetSuccess(string.Format("Xóa [{0}] khách hàng thành công",ids.Length));
            }
            else
            {
                SetError(string.Format("Xóa [{0}] khách hàng không thành công",ids.Length));
            }

            return GetResultOrReferrerDefault(defauthPath);
            // var
        }

        private bool IsValidate(Customer customer)
        {
            if (CustomerRepository.UseInstance.FieldExist("Name",customer.Name,customer.ID))
            {
                SetError("Tên khách hàng đã tồn tại");
            }

            return !HasError;
        }

    }
}