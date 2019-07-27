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
    public class CategoryRepository : Category
    {
        public static List<Category> Search(SearchParam param, Pagination paging)
        {
            var sql = Sql.Builder
                         .Where("IDCategoryType = @0", param.Type)
                         .Where("Parent = @0", param.Parent);
            if (!param.Term.IsNullOrEmpty())
            {
                sql.Where(string.Format("SearchMeta like '%{0}%'", param.Term.RemoveUnicode()));
            }
            if (Equals(paging, null))
                return UseInstance.GetListOrDefault(sql);
            else
                return UseInstance.GetListOrDefault(sql, paging);
        }

        public static bool NameExist(int type, int parent, string name, int id = 0)
        {
            try
            {
                var sql = Sql.Builder
                             .Where("IDCategoryType =@0 and Parent=@1 and Name=N'@2' and ID<>@3", type, parent, name, id);
                var categoryInstance = Instance.SingleOrDefault<Category>(sql);
                return categoryInstance.ID > 0 ? true : false;
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return false;
            }
        }
        public static bool DelChildNotParent()
        {
            try
            {
                var sql = Sql.Builder
                             .Where("Parent > 0")
                             .Where(string.Format("Parent NOT IN (SELECT ID FROM {0})", UseInstance.GetTableName()));
                return Instance.Delete<Category>(sql) > 0 ? true : false;
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return false;
            }

        }

    }
}