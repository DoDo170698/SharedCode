using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.PurchaseManage;
using System.Collections.Generic;

namespace QuanLyHoaDon.Models.Views
{
    public class OrderDiscountModel
    {
        public List<OrderDiscount> OrderDiscounts { get; set; }
        public OrderDiscount OrderDiscount { get; set; }
        public List<Customer> Customers { get; set; }
        public Dictionary<int,string> Quaters { get; set; }
        public List<Order> Orders { get; set; }
        public List<int> Years { get; set; }
        public Dictionary<int, string> OrderTypes { get; set; }
        public OrderDiscountParam SearchParam { get; set; }
        public string Url { get; set; }
    }
}