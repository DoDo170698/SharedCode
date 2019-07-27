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
    public class OrderFileRepository : OrderFile
    {
        public static List<OrderFile> Search(OrderFileParam param, Pagination paging)
        {
            var sql = Sql.Builder
                         .Where("IDOrder =@0",param.IDOrder);
            if (!param.Types.IsNullOrEmpty())
            {
                sql.Where("Type in (@0)", param.Types);
            }
            return UseInstance.GetListOrDefault(sql, paging);
        }
        public static OrderFile GetFirstOrDefault(OrderFileParam param)
        {
            var sql = Sql.Builder
                         .Where("IDOrder =@0", param.IDOrder);
            if (!param.Types.IsNullOrEmpty())
            {
                sql.Where("Type in (@0)", param.Types);
            }
            return UseInstance.GetFirstByFieldOrdefault(sql);
        }
    }
}