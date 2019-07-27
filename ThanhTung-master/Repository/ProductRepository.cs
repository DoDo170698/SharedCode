using NPoco;
using QuanLyHoaDon.CodeLogic;
using QuanLyHoaDon.CodeLogic.Commons;
using QuanLyHoaDon.Models.Admin;
using QuanLyHoaDon.Models.PurchaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Repository
{
    public class ProductRepository : Product
    {
        public static List<Product> Search(SearchParam param, Pagination paging)
        {
            var sql = Sql.Builder;
            if (!Equals(param.Term, string.Empty))
            {
               sql.Where(string.Format("SearchMeta like '%{0}%'", param.Term.RemoveUnicode()));
            }
            return UseInstance.GetListOrDefault(sql, paging);
        }
    }
}