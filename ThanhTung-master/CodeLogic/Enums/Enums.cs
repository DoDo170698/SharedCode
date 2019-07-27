using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QuanLyHoaDon.CodeLogic.Enums
{
    public class Enums
    {
        public enum CompareTypes
        {
            Equal = 1,
            NotEqual = 2,
            GreaterThan = 3,
            LowerThan = 4,
            In = 5,
        }
        public enum TypeSQl
        {
            String = 1,
            Number = 2
        }
        public enum TypeObject
        {
            Byte = 0,
            Int = 1,
            Long = 2,
            String = 3,
            Char = 4,
            Float = 5,
            Double = 7,
            DateTime = 8,
            DateTimeNull = 9
        }
        public enum OrderStatus
        {
            [Description("Nhận đơn")]
            Receive = 1,
            [Description("Giao hàng")]
            Delivery = 2,
            [Description("Trả phiếu nhập kho")]
            IRVReturn = 3,
            [Description("Thu thập chứng từ")]
            CollectVouchers = 4,
            [Description("Đã gửi chứng từ")]
            AccountingDept = 5,
            [Description("Xác nhận chứng từ")]
            AccountingDeptConfirm = 6,
            [Description("Thanh toán")]
            Payed = 7,
            [Description("Xác nhận thanh toán")]
            PayedConfirm = 8
        }
        public enum MvcStringRender
        {
            OrderStatus = 1,
            AccountingDeptStatus = 2,
            OrderType = 3,
            Quater =4
        }
        public enum ActionStatus
        {
            [Description("Tạo mới")]
            Create = 1,
            [Description("Cập nhật")]
            Update = 2,
            [Description("Xóa")]
            Delete = 3,
            [Description("Xem")]
            View = 4,
        }
        public enum OrderFileType
        {
            [Description("File trả PNK")]
            IRV = 1,
            [Description("File đơn hàng")]
            Order = 2,
            [Description("File kê khai chi tiết")]
            Declaration = 3,
            [Description("File hóa đơn")]
            Bill = 4,
        }
        public enum AccountingDeptStatus
        {
            [Description("Đã nhận")]
            Receive = 1,
            [Description("Đã trả lại")]
            Reject = 2,
            [Description("Đã xác nhận")]
            Confirm = 3,
        }
        public enum OrderType
        {
            [Description("Đơn hàng")]
            Order = 1,
            [Description("Số lượng")]
            Amount = 2,
        }
        public enum Quater
        {
            [Description("Quý 1")]
            Quater1 = 1,
            [Description("Quý 2")]
            Quater2 = 2,
            [Description("Quý 3")]
            Quater3 = 3,
            [Description("Quý 4")]
            Quater4 = 4,
            [Description("Cả năm")]
            All = 5,
        }
        public enum IModule
        {
            [Description("Quản lý hóa đơn")]
            QLHD_Manage = 1,
            [Description("Xem hóa đơn")]
            QLHD_Viewer=2
        }
    }
}