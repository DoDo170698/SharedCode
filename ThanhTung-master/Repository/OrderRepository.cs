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
    public class OrderRepository : Order
    {
        public static List<Order> Search(SearchParam param, Pagination paging)
        {
            var sql = Sql.Builder;
            sql.Where("Type = @0", param.Type);
            if (!param.Term.IsNullOrEmpty())
            {
               sql.Where(string.Format("SearchMeta like '%{0}%'", param.Term.RemoveUnicode()));
            }
            if (!param.ListStatus.IsNullOrEmpty() && param.ListStatus[0]!=0)
            {
                sql.Where("Status in (@0)",param.ListStatus);
            }
            if (param.Status >0)
            {
                sql.Where("Status = @0", param.Status);
            }
            if (!Equals(param.StartDate,null) && !Equals(param.EndDate,null))
            {

                sql.Where("((PODate >= @startDate AND PODate <= @endDate) OR (DeliveryDate >= @startDate AND DeliveryDate <= @endDate) OR (PODate <= @startDate AND DeliveryDate >= @endDate))", new
                {
                    startDate = param.StartDate,
                    endDate = param.EndDate
                });
            }
            else if (!Equals(param.StartDate,null))
            {
                sql.Where("(@startDate <= DeliveryDate)", new
                {
                    startDate = param.StartDate
                });
            }
            else if (!Equals(param.EndDate,null))
            {
                sql.Where("(PODate <= @endDate)", new
                {
                    endDate = param.EndDate
                });
            }
            if (!Equals(param.IRVReturnDateDate,null))
            {
                sql.Where("IRVReturnDate =@0", param.IRVReturnDateDate.Value.Date);
            }
            if (!Equals(param.PayedDate, null))
            {
                sql.Where(string.Format("convert(varchar(10),PayedDate, 103) = '{0}'",Utils.DateToString(param.PayedDate, "dd/MM/yyyy")));
            }
            sql.OrderBy("PODate DESC");
            return UseInstance.GetListOrDefault(sql, paging);
        }
    }
}