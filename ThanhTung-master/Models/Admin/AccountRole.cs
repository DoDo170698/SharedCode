using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Admin
{
    public class AccountRole
    {
        public int ID { get; set; }
        public int IDChannel { get; set; }
        public int IDRole { get; set; }
        public int IDAccount { get; set; }
        public string IDModules { get; set; }
        public DateTime ? Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ? Updated { get; set; }
        public int UpdatedBy { get; set; }
    }
}