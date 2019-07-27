using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static QuanLyHoaDon.CodeLogic.Enums.Enums;

namespace QuanLyHoaDon.CodeLogic.Customs
{
    public class CUtils
    {
        public static MvcHtmlString RenderOderStatus(int status, string addCss = "white d-block")
        {
            var dic = Utils.EnumToDictionary<OrderStatus>();
            TagBuilder tag = new TagBuilder("span");
            tag.MergeAttribute("value", status.ToString());
            switch (status)
            {
                case (int)OrderStatus.Receive:
                    tag.SetInnerText(dic[(int)OrderStatus.Receive]);
                    tag.MergeAttribute("class", "label label-default " + addCss);
                    break;
                case (int)OrderStatus.Delivery:
                    tag.SetInnerText(dic[(int)OrderStatus.Delivery]);
                    tag.MergeAttribute("class", "label label-warning " + addCss);
                    break;
                case (int)OrderStatus.IRVReturn:
                    tag.SetInnerText(dic[(int)OrderStatus.IRVReturn]);
                    tag.MergeAttribute("class", "label label-green " + addCss);
                    break;
                case (int)OrderStatus.CollectVouchers:
                    tag.SetInnerText(dic[(int)OrderStatus.CollectVouchers]);
                    tag.MergeAttribute("class", "label label-primary " + addCss);
                    break;
                //case (int)OrderStatus.AccountingDept:
                //    tag.SetInnerText(dic[(int)OrderStatus.AccountingDept]);
                //    tag.MergeAttribute("class", "label label-violet " + addCss);
                //    break;
                //case (int)OrderStatus.AccountingDeptConfirm:
                //    tag.SetInnerText(dic[(int)OrderStatus.AccountingDeptConfirm]);
                //    tag.MergeAttribute("class", "label label-blueviolet " + addCss);
                //    break;
                case (int)OrderStatus.Payed:
                    tag.SetInnerText(dic[(int)OrderStatus.Payed]);
                    tag.MergeAttribute("class", "label label-success " + addCss);
                    break;
                case (int)OrderStatus.PayedConfirm:
                    tag.SetInnerText(dic[(int)OrderStatus.PayedConfirm]);
                    tag.MergeAttribute("class", "label label-darksuccess " + addCss);
                    break;
                //case (int)OrderStatus.Execellent:
                //    tag.SetInnerText(dic[(int)LevelCourse.Execellent]);
                //    tag.MergeAttribute("class", "label label-purple  " + addCss);
                //    break;
                default:
                    break;
            }

            return new MvcHtmlString(tag.ToString());
        }
        public static MvcHtmlString RenderACDStatus(int status, string addCss = "white d-block")
        {
            var dic = Utils.EnumToDictionary<AccountingDeptStatus>();
            TagBuilder tag = new TagBuilder("span");
            tag.MergeAttribute("value", status.ToString());
            switch (status)
            {
                case (int)AccountingDeptStatus.Receive:
                    tag.SetInnerText(dic[(int)AccountingDeptStatus.Receive]);
                    tag.MergeAttribute("class", "label label-success " + addCss);
                    break;
                case (int)AccountingDeptStatus.Reject:
                    tag.SetInnerText(dic[(int)AccountingDeptStatus.Reject]);
                    tag.MergeAttribute("class", "label label-danger " + addCss);
                    break;
                case (int)OrderStatus.IRVReturn:
                    tag.SetInnerText(dic[(int)AccountingDeptStatus.Confirm]);
                    tag.MergeAttribute("class", "label label-darksuccess " + addCss);
                    break;
                default:
                    break;
            }

            return new MvcHtmlString(tag.ToString());
        }
        public static MvcHtmlString RenderOrderType(int status, string addCss = "white d-block")
        {
            var dic = Utils.EnumToDictionary<OrderType>();
            TagBuilder tag = new TagBuilder("span");
            tag.MergeAttribute("value", status.ToString());
            switch (status)
            {
                case (int)OrderType.Order:
                    tag.SetInnerText(dic[(int)OrderType.Order]);
                    tag.MergeAttribute("class", "label label-warning " + addCss);
                    break;
                case (int)OrderType.Amount:
                    tag.SetInnerText(dic[(int)AccountingDeptStatus.Reject]);
                    tag.MergeAttribute("class", "label label-primary " + addCss);
                    break;

                default:
                    break;
            }

            return new MvcHtmlString(tag.ToString());
        }
        public static MvcHtmlString RenderQuater(int status, string addCss = "white d-block")
        {
            var dic = Utils.EnumToDictionary<Quater>();
            TagBuilder tag = new TagBuilder("span");
            tag.MergeAttribute("value", status.ToString());
            switch (status)
            {
                case (int)Quater.Quater1:
                    tag.SetInnerText(dic[(int)Quater.Quater1]);
                    tag.MergeAttribute("class", "label label-warning " + addCss);
                    break;
                case (int)Quater.Quater2:
                    tag.SetInnerText(dic[(int)Quater.Quater2]);
                    tag.MergeAttribute("class", "label label-primary " + addCss);
                    break;
                case (int)Quater.Quater3:
                    tag.SetInnerText(dic[(int)Quater.Quater3]);
                    tag.MergeAttribute("class", "label label-success " + addCss);
                    break;
                case (int)Quater.Quater4:
                    tag.SetInnerText(dic[(int)Quater.Quater4]);
                    tag.MergeAttribute("class", "label label-danger " + addCss);
                    break;
                case (int)Quater.All:
                    tag.SetInnerText(dic[(int)Quater.All]);
                    tag.MergeAttribute("class", "label label-violet " + addCss);
                    break;
                default:
                    break;
            }

            return new MvcHtmlString(tag.ToString());
        }
        public static string GetTitleByView(string view)
        {
            var title = "Danh sách đơn ";
            switch (view)
            {
                case "Receive":
                    title += "đặt hàng";
                    break;
                case "Delivery":
                    title += "giao hàng";
                    break;
                case "IRVReturn":
                    title += "đã trả phiếu nhập kho";
                    break;
                case "CollectVouchers":
                    title += " đã thu thập chứng từ";
                    break;
                case "AccountingDept":
                    title += "đã chuyển đến phòng kế toán";
                    break;
                case "Payed":
                    title += "đã thanh toán hóa đơn";
                    break;
                default:
                    break;
            }
            return title;
        }
        public static string GetLinkByStatus(int status, string controller)
        {
            var link = "#";
            switch (status)
            {
                case (int)OrderStatus.Receive:
                    link = string.Format("/{0}?ViewName={1}&ListStatus={2}", controller, "Receive", (int)OrderStatus.Receive);
                    break;
                case (int)OrderStatus.Delivery:
                    link = string.Format("/{0}?ViewName={1}&ListStatus={2}", controller, "Delivery", (int)OrderStatus.Delivery);
                    break;
                case (int)OrderStatus.IRVReturn:
                    link = string.Format("/{0}?ViewName={1}&ListStatus={2}", controller, "IRVReturn", (int)OrderStatus.IRVReturn);
                    break;
                case (int)OrderStatus.CollectVouchers:
                    link = string.Format("/{0}?ViewName={1}&ListStatus={2}", controller, "CollectVouchers", (int)OrderStatus.CollectVouchers);
                    break;
                //case (int)OrderStatus.AccountingDept:
                //case (int)OrderStatus.AccountingDeptConfirm:
                //    var adStatus = string.Format("{0},{1}", (int)OrderStatus.AccountingDept, (int)OrderStatus.AccountingDeptConfirm);
                //    link = string.Format("/{0}?ViewName={1}&ListStatus={2}", "AccountingDept", adStatus);
                //    break;
                case (int)OrderStatus.Payed:
                case (int)OrderStatus.PayedConfirm:
                    var payedStatus = string.Format("{0},{1}", (int)OrderStatus.Payed, (int)OrderStatus.PayedConfirm);
                    link = string.Format("/{0}?ViewName={1}&ListStatus={2}", controller, "CollectVouchers", payedStatus);
                    break;
                default:
                    break;
            }
            return link;
        }
        public static string GetListStatus(int status)
        {
            var list = "0";
            switch (status)
            {
                case (int)OrderStatus.Receive:
                    list = string.Format("{0}", (int)OrderStatus.Receive);
                    break;
                case (int)OrderStatus.Delivery:
                    list = string.Format("{0}", (int)OrderStatus.Delivery);
                    break;
                case (int)OrderStatus.IRVReturn:
                    list = string.Format("{0}", (int)OrderStatus.IRVReturn);
                    break;
                case (int)OrderStatus.CollectVouchers:
                    list = string.Format("{0}", "CollectVouchers", (int)OrderStatus.CollectVouchers);
                    break;
                case (int)OrderStatus.AccountingDept:
                case (int)OrderStatus.AccountingDeptConfirm:
                    list = string.Format("{0},{1}", (int)OrderStatus.AccountingDept, (int)OrderStatus.AccountingDeptConfirm);
                    break;
                case (int)OrderStatus.Payed:
                case (int)OrderStatus.PayedConfirm:
                    list = string.Format("{0},{1}", (int)OrderStatus.Payed, (int)OrderStatus.PayedConfirm);
                    break;
                default:
                    break;
            }
            return list;
        }
        public static int GetOverDay(DateTime realDay, DateTime oldDay)
        {
            try
            {
                var overDay = 0;
                overDay = (realDay.Date - oldDay.Date).Days;
                return overDay;
            }
            catch (Exception e)
            {
                var mess = e.Message;
                return 0;
            }

        }
        public static bool ExportToPdf(DataTable data, string template, out string filePdf,string header,string footer, float[] widths)
        {
            filePdf = string.Empty;
            try
            {
                Document document = new Document();
                filePdf = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\" + template);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePdf, FileMode.Truncate));
                document.Open();
                PdfPTable table = new PdfPTable(data.Columns.Count);
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                if (!Equals(null, widths))
                    table.SetWidths(widths);
                
                string fonts = Path.Combine(AppDomain.CurrentDomain.BaseDirectory) + @"\Assets\fonts\arialbd.ttf";
                BaseFont bf = BaseFont.CreateFont(fonts, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 8);
                //header
                if (!header.IsNullOrEmpty())
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, new Font(bf, 14)));
                    headerCell.Colspan = data.Columns.Count;
                    headerCell.Border = 0;
                    headerCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    headerCell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    headerCell.PaddingBottom = 20;
                    table.AddCell(headerCell);
                }
                //column
                for (int k = 0; k < data.Columns.Count; k++)
                {

                    PdfPCell cell = new PdfPCell(new Phrase(data.Columns[k].ColumnName, font));
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.BackgroundColor = new BaseColor(188, 223, 241);
                    cell.Padding = 5;
                    table.AddCell(cell);
                }
                // row
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        var caption = data.Columns[i].Caption;

                        PdfPCell cell = new PdfPCell(new Phrase(data.Rows[i][j].ToString(), font));
                        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        cell.VerticalAlignment = PdfPCell.ALIGN_LEFT;
                        cell.Padding = 5;
                        cell.NoWrap = true;
                        table.AddCell(cell);
                    }
                }
                // footer
                if (!footer.IsNullOrEmpty())
                {
                    PdfPCell footerCell = new PdfPCell(new Phrase(footer, font));
                    footerCell.Colspan = data.Columns.Count;
                    footerCell.Border = 0;
                    footerCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    footerCell.VerticalAlignment = PdfPCell.ALIGN_LEFT;
                    footerCell.PaddingTop = 10;
                    table.AddCell(footerCell);
                }
                document.Add(table);
                document.Close();
                return true;
            }
            catch (Exception ex)
            {
                var mess = ex.Message;
                return false;
            }
        }
    }
}