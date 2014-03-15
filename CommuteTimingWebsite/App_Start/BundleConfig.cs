using System.Web;
using System.Web.Optimization;

namespace CommuteTimingWebsite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/font-awesome.css",
                "~/Content/site.css",
                "~/Content/toastr.css"
                ,"~/Content/fontello.css"
                , "~/Content/datetimepicker.css"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include("~/Scripts/angular-animate.js","~/Scripts/angular-myServices.js","~/Scripts/angular-mapsDirective.js"));
        }
    }
}