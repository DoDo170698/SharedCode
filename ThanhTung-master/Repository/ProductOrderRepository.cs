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
    public class ProductOrderRepository : ProductOrder
    {
        public static List<ProductOrder> Search(ProductOrderParam param)
        {
            var sql = Sql.Builder
                         .Where("IDProduct = @0", param.IDProduct)
                         .Where("IDOrder = @0", param.IDOrder);
            return UseInstance.GetListOrDefault(sql);
        }
        public static bool DeleteByFieldLongIDs(string field, long[] ids)
        {
            try
            {
                var sql = Sql.Builder
                .Where(string.Format("{0} in ({1})", field, Utils.GetStringJoin(",", ids)));

                return Instance.Delete<ProductOrder>(sql) > 0 ? true : false;
            }
            catch (Exception e)
            {
                var x = e.Message;
                return false;
            }

        }
    }
}