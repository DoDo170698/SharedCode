using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Admin
{
    public class Module
    {
        public int ID { get; set; }
        public int IDChannel { get; set; }
        public int IDRole { get; set; }
        public string Code { get; set; }
        public int Parent { get; set; }
        public string Parents { get; set; }
        public DateTime ? Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ? Updated { get; set; }
        public int UpdatedBy { get; set; }
    }
}