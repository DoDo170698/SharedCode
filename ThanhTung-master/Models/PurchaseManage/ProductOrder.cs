using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.PurchaseManage
{
    [TableName("ProductOrder")]
    [PrimaryKey("ID")]
    public class ProductOrder : PurchaseManageDataContext<ProductOrder>
    {
        [Column]
        public int ID { get; set; }
        [Column]
        public int IDChannel { get; set; }
        [Column]
        public int IDOrder { get; set; }
        [Column]
        public int IDProduct { get; set; }
        [Column]
        public int Amount { get; set; }
        [Column]
        public long Total { get; set; }
        [Column]
        public int RealAmount { get; set; }
        [Column]
        public long RealTotal { get; set; }
        [Column]
        public long Promotion { get; set; }
        [Column]
        public long RealPromotion { get; set; }
        [Column]
        public int CreatedBy { get; set; }
        [Column]
        public DateTime? Updated { get; set; }
        [Column]
        public int UpdatedBy { get; set; }
    }
}