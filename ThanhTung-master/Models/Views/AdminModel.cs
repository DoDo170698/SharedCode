using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Views
{
    public class AdminModel
    {
        public List<Account> Accounts { get; set; }
        public Account Account { get; set; }
        public CategoryType CategoryType { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public Category Category { get; set; }
        public List<Category> Categories { get; set; }
        public List<Category> Ancestors { get; set; }
        public Category Parent { get; set; }
        public SearchParam SearchParam { get; set; }
        public string Url { get; set; }
    }
}