using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.PurchaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Views
{
    public class OrderModel
    {
        public List<Order> Orders { get; set; }
        public Order Order { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductOrder> ProductOrders { get; set; }
        public List<Category> CalUnits { get; set; }
        public List<Customer> Customers { get; set; }
        public Customer Customer { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public Supplier Supplier { get; set; }
        public List<OrderFile> OrderFiles { get; set; }
        public OrderFile OrderFile { get; set; }
        public Dictionary<int,string> ListStatus { get; set; }
        public int Status { get; set; }
        public string Tab { get; set; }
        public string ViewName { get; set; }
        public List<int> IDOrders { get; set; }
        public SearchParam SearchParam { get; set; }
        public int Action { get; set; }
        public bool IsAD { get; set; }

        public string Url { get; set; }
    }
}