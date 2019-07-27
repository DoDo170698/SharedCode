using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace QuanLyHoaDon
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Bootstrap
            bundles.Add(new StyleBundle("~/Assets/bootstrap/css.css").Include(
                "~/Assets/bootstrap/css/bootstrap.css",
                "~/Assets/bootstrap/css/bootstrap-theme.css"
                ));
            bundles.Add(new ScriptBundle("~/Assets/bootstrap/js.js").Include(
                "~/Assets/bootstrap/js/bootstrap.js"
            ));
            //Beyond admin 
            bundles.Add(new StyleBundle("~/Assets/beyond/css.css").Include(
              "~/Assets/beyond/css/font-awesome.css",
              "~/Assets/beyond/css/weather-icons.css",
              "~/Assets/beyond/css/beyond.css",
              "~/Assets/beyond/css/demo.css",
              "~/Assets/beyond/css/typicons.css",
              "~/Assets/beyond/css/animate.css",
              "~/Assets/beyond/css/dataTables.bootstrap.css"
              ));
            bundles.Add(new ScriptBundle("~/Assets/beyond/js.js").Include(
                  "~/Assets/beyond/js/slimscroll/jquery.slimscroll.js",
                  "~/Assets/beyond/js/validation/bootstrapValidator.js",
                  "~/Assets/beyond/js/editors/summernote/summernote.js",
                  "~/Assets/beyond/js/nestable/jquery.nestable.min.js",

                  "~/Assets/beyond/js/charts/easypiechart/jquery.easypiechart.js",
                  "~/Assets/beyond/js/charts/easypiechart/easypiechart-init.js",

                  "~/Assets/beyond/js/charts/flot/jquery.flot.js",
                  "~/Assets/beyond/js/charts/flot/jquery.flot.resize.js",
                  "~/Assets/beyond/js/charts/flot/jquery.flot.pie.js",
                  "~/Assets/beyond/js/charts/flot/jquery.flot.tooltip.js",
                  "~/Assets/beyond/js/charts/flot/jquery.flot.orderBars.js",

                  "~/Assets/beyond/js/skins.js",
                  "~/Assets/beyond/js/beyond.js"
              ));

            //JQuery
            bundles.Add(new StyleBundle("~/Assets/jquery/css.css").Include(
                "~/Assets/jquery/css/jquery.ui.css",
                "~/Assets/jquery/css/jquery.cscrollbar.css",
                "~/Assets/jquery/css/jquery.inputbox.css",
                "~/Assets/jquery/css/jquery.datetimepicker.css",
                "~/Assets/jquery/css/jquery.bootstrap-select.css",
                "~/Assets/jquery/css/jquery.jcrop.css",
                "~/Assets/jquery/css/jquery.contextMenu.css",
                "~/Assets/jquery/css/jquery.megaimgviewer.css",
                "~/Assets/jquery/css/jquery.select2bootstrap.css",
                "~/Assets/jquery/css/jquery.select2.css",
                "~/Assets/jquery/css/jquery.owlCarousel.css",
                "~/Assets/jquery/css/jquery.selectbox.css",
                "~/Assets/jquery/css/jquery.rating.css"
                ));
            bundles.Add(new ScriptBundle("~/Assets/jquery/js.js").Include(
                "~/Assets/jquery/js/jquery.js",
                "~/Assets/jquery/js/jquery.mousewheel.js",
                "~/Assets/jquery/js/jquery.cscrollbar.js",
                "~/Assets/jquery/js/jquery.ui.js",
                "~/Assets/jquery/js/jquery.select2.js",
                "~/Assets/jquery/js/jquery.jcrop.js",
                "~/Assets/jquery/js/jquery.datetimepicker.js",
                "~/Assets/jquery/js/jquery.bootstrap-select.js",
                "~/Assets/jquery/js/jquery.serializejson.js",
                "~/Assets/jquery/js/jquery.rating.js"
                ));

            bundles.Add(new ScriptBundle("~/Assets/highcharts/js.js").Include(
                "~/Assets/highcharts/js/highcharts.js",
                "~/Assets/highcharts/js/modules/exporting.js",
                "~/Assets/highcharts/js/modules/canvas-tools.js"
                ));
            //PDF
            bundles.Add(new StyleBundle("~/pdf/css.css").Include(
                "~/pdf/viewer.css"
                ));
            bundles.Add(new ScriptBundle("~/pdf/js.js").Include(
                "~/pdf/compatibility.js",
                "~/pdf/l10n.js",
                "~/pdf/build/pdf.js",
                "~/pdf/viewer.js"
                ));

            //App
            var mycss = new List<string>
            {
                "~/Assets/app/css/main.css",
                "~/Assets/app/css/size.css",
                "~/Assets/app/css/stg.css",
                "~/Assets/app/css/wf.css",
                "~/Assets/app/css/cmt.css",
                "~/Assets/app/css/ntfs.css",
                "~/Assets/app/css/cust.css",
                "~/Assets/app/css/mailtip.css"
            };
            bundles.Add(new StyleBundle("~/Assets/app/css.css").Include(mycss.ToArray()));
            bundles.Add(new ScriptBundle("~/Assets/app/js.js").Include(
                "~/Assets/app/js/mailtip.js",
                "~/Assets/app/js/autocomplete.js",
                "~/Assets/app/js/utils.js",
                "~/Assets/app/js/main.js",
                "~/Assets/app/js/uploader.js",
                "~/Assets/app/js/upfile.js",
                "~/Assets/app/js/admin.js",
                "~/Assets/app/js/avatar.js",
                "~/Assets/app/js/pages.js",
                "~/Assets/app/js/test.js",
                "~/Assets/app/js/cust.js",
                "~/Assets/app/js/tribunate.js",
                "~/Assets/app/js/qlns.js"
            ));
            BundleTable.EnableOptimizations = false;
        }
    }
}
