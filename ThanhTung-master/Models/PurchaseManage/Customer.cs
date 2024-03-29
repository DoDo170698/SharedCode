﻿using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.Models.PurchaseManage
{
    [TableName("Customer")]
    [PrimaryKey("ID")]
    public class Customer : PurchaseManageDataContext<Customer>
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
        public string Phone { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public string Address { get; set; }
        [Column]
        public DateTime? Created { get; set; }
        [Column]
        public int CreatedBy { get; set; }
        [Column]
        public DateTime? Updated { get; set; }
        [Column]
        public int UpdatedBy { get; set; }

        public string SearchMeta { get; set; }
       
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