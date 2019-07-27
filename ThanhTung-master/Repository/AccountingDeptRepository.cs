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
    public class AccountingDeptRepository : AccountingDept
    {
        public static List<AccountingDept> Search(AccountingDeptParam param, Pagination paging)
        {
            var sql = Sql.Builder;
            if (!param.Term.IsNullOrEmpty())
            {
                sql.Where(string.Format("IDOrder in (Select ID From [dbo].[Order] Where SearchMeta like '%{0}%')", param.Term.RemoveUnicode()));
            }
            if (param.OrderType > 0)
            {
                sql.Where("Type =@0", param.OrderType);
            }
            if (param.Status > 0)
            {
                sql.Where("Status = @0", param.Status);
            }
            if (!Equals(param.StartDate, null) && !Equals(param.EndDate, null))
            {

                sql.Where("(Created >= @startDate AND Created <= @endDate)", new
                {
                    startDate = param.StartDate,
                    endDate = param.EndDate
                });
            }
            else if (!Equals(param.StartDate, null))
            {
                sql.Where("(@startDate <= Created)", new
                {
                    startDate = param.StartDate
                });
            }
            else if (!Equals(param.EndDate, null))
            {
                sql.Where("(Created <= @endDate)", new
                {
                    endDate = param.EndDate
                });
            }
            return UseInstance.GetListOrDefault(sql, paging);
        }
    }
}