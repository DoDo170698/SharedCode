using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.PurchaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Views
{
    public class CustomerModel
    {
        public List<Customer> Customers { get; set; }
        public Customer Customer { get; set; }
        public SearchParam SearchParam { get; set; }
        public string Url { get; set; }
    }
}