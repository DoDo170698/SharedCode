using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.CodeLogic.Commons
{
    public class OrderDiscountParam : SearchParam
    {
        public int IDCustomer { get; set; }
        public int Quater { get; set; }
        public int Year { get; set; }
    }
}