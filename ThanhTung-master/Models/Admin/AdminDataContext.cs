using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Admin
{
    public class AdminDataContext<T> : ModelDataContext<T> where T : class, new()
    {
        public AdminDataContext(string connectString = "CloudConnectString") : base(connectString)
        {
        }
    }
}