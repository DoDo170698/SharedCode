using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Admin
{
    [TableName("CategoryType")]
    [PrimaryKey("ID")]
    public class CategoryType : AdminDataContext<CategoryType>
    {
        [Column]
        public int ID { get; set; }
        [Column]
        public int IDChannel { get; set; }
        [Column]
        public string Code { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public string Describes { get; set; }
        [Column]
        public string SearchMeta { get; set; }
        [Column]
        public DateTime ? Created { get; set; }
        [Column]
        public int CreatedBy { get; set; }
        public DateTime ? Updated { get; set; }
        [Column]
        public int UpdatedBy { get; set; }
        [Ignore]
        public List<string> FieldSearchs
        {
            get
            {
                return new List<string>
            {
                    "Name","Code"
            };
            }
        }
    }
}