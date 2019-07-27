using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.PurchaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Views
{
    public class AccountingDeptModel
    {
        public List<AccountingDept> AccountingDepts { get; set; }
        public List<Order> Orders { get; set; }
        public Order Order { get; set; }
        public List<OrderFile> OrderFiles { get; set; }
        public AccountingDeptParam SearchParam { get; set; }
        public Dictionary<int, string> ListStatus{ get; set; }
        public int Action { get; set; }
        public string Url { get; set; }
    }
}