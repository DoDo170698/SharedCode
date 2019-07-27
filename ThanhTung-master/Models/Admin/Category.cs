using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Admin
{
    [TableName("Category")]
    [PrimaryKey("ID")]
    public class Category : AdminDataContext<Category>
    {
        [Column]
        public int ID { get; set; }
    
        [Column]
        public int IDChannel { get; set; }
        [Column]
        public int IDCategoryType { get; set; }
        [Column]
        public string Code { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public int Parent { get; set; }
        [Column]
        public string Parents { get; set; }
        [Column]
        public string Describes { get; set; }
        [Column]
        public DateTime ? Created { get; set; }
        [Column]
        public int CreatedBy { get; set; }
        public DateTime ? Updated { get; set; }
        [Column]
        public int UpdatedBy { get; set; }
        [Column]
        public string SearchMeta { get; set; }
        [Ignore]
        public List<string> FieldSearchs
        {
            get
            {
                return new List<string>
            {
                    "Name","Code","Describes"
            };
            }
        }
    }
}