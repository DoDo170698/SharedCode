using QuanLyHoaDon.CodeLogic;
using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Controllers;
using QuanLyHoaDon.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyHoaDon.Controllers
{
    public class AutocompleteController : BaseController
    {
        public JsonResult Customer()
        {
            var items = new List<object>();
            var param = new SearchParam().BindData(DATA);

            var customers = CustomerRepository.Search(param,null);
            if (!Equals(customers,null))
            {
                customers = customers.OrderByDescending(n => n.ID).ToList();
                foreach (var item in customers)
                {
                    items.Add(new
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Parents = string.Empty
                    });
                }
            }
            SetOnlyDataResponse(items);
            return GetResult();
        }
        public JsonResult Supplier()
        {
            var items = new List<object>();
            var param = new SearchParam().BindData(DATA);

            var suppliers = SupplierRepository.Search(param, null);
            if (!Equals(suppliers, null))
            {
                suppliers = suppliers.OrderByDescending(n => n.ID).ToList();
                foreach (var item in suppliers)
                {
                    items.Add(new
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Parents = string.Empty
                    });
                }
            }
            SetOnlyDataResponse(items);
            return GetResult();
        }
        public JsonResult Category()
        {
            var items = new List<object>();
            var param = new SearchParam().BindData(DATA);
            var categories = CategoryRepository.Search(param, null);
            if (!Equals(categories, null))
            {
                categories = categories.OrderByDescending(n => n.ID).ToList();
                foreach (var item in categories)
                {
                    items.Add(new
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Parents = string.Empty
                    });
                }
            }
            SetOnlyDataResponse(items);
            return GetResult();
        }

    }
}