using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.Views
{
    public class BaseModel
    {
        public int ID { get; set; }
        public string IDs { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Target { get; set; } = "";
        public string Class { get; set; } = "";
        public bool IsMulti { get; set; } = false;
    }
}