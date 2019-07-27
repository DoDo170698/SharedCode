using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Admin
{
    [TableName("Account")]
    [PrimaryKey("ID")]
    public class Account:AdminDataContext<Account>
    {
        [Column]
        public int ID { get; set; }
        [Column]
        public int IDChannel { get; set; }
        [Column]
        public string Code { get; set; }
        [Column]
        public string UserName { get; set; }
        [Column]
        public string PassWord { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Phone { get; set; }
        [Column]
        public string Address { get; set; }
        [Column]
        public string Roles { get; set; }
        [Column]
        public bool IsAdmin { get; set; }
    }
}