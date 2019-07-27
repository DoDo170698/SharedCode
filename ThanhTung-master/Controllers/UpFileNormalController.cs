using QuanLyHoaDon.CodeLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyHoaDon.Controllers
{
    public class UpFileNormalController : BaseController
    {
        // GET: UpFile
        [HttpPost]
        public ActionResult UpFile()
        {
            var file = Request.Files[0];
            var target = Request.Form["Target"];
            var fileName = DateTime.Now.ToString("ddMMyyyyHHmmss");
            var fullPath = "";
            var path = "";
            var down = "";
            SystemConfig.GetValueByKey("FileFolder");
            var realPath = SystemConfig.GetValueByKey("FileFolder");
            if (file.ContentLength > 0)
            {
                var ftmp = DateTime.Now.ToString("ddMMyyyssmmhh");
                down = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, realPath, ftmp, file.FileName.Reduced());
                path = ftmp + @"\" + Path.GetFileName(file.FileName.Reduced());
                fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,realPath, path);//Path.Combine(realPath, path);
                Directory.CreateDirectory(Path.GetDirectoryName(down));
                file.SaveAs(down);
            }
            return Json(new
            {
                FileName = file.FileName,
                FullPath = down,
                Path = path,
                Target = target,
            });
        }
        public ActionResult Download()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,Utils.GetString(DATA, "Path"));
            var fileName = Utils.GetString(DATA, "FileName");
            if (IsAjax)
            {
                var fullPath = string.Format("/UpFileNormal/download?Path={0}&FileName={1}", path, fileName);
                return Json(new
                {
                    FullPath = fullPath,
                });
            }
            else
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(@path);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }

        }
    }
}