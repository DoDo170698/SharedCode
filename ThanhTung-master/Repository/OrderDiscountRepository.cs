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
    public class OrderDiscountRepository : OrderDiscount
    {
        public static List<OrderDiscount> Search(OrderDiscountParam param, Pagination paging)
        {
            var sql = Sql.Builder;
            if (!Equals(param.Term, string.Empty))
            {
                sql.Where(string.Format("SearchMeta like '%{0}%'", param.Term.RemoveUnicode()));
            }
            if (param.IDCustomer >0 )
            {
                sql.Where("IDCustomer =@0",param.IDCustomer);
            }
            if (param.Year >0)
            {
                sql.Where("Year =@0", param.Year);
            }
            if (param.Quater > 0)
            {
                sql.Where("Quater =@0", param.Quater);
            }
            if (param.Type > 0)
            {
                sql.Where("Type =@0", param.Type);
            }
            return UseInstance.GetListOrDefault(sql, paging);
        }
    }
}