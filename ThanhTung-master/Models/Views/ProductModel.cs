using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.PurchaseManage;
using System.Collections.Generic;

namespace QuanLyHoaDon.Models.Views
{
    public class ProductModel
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public Category ProductType { get; set; }
        public  List<Category> ProductTypes { get; set; }
        public Category CalUnit { get; set; }
        public SearchParam SearchParam { get; set; }
        public string Url { get; set; }
    }
}