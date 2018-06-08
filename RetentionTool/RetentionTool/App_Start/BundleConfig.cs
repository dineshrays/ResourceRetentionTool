using System.Web;
using System.Web.Optimization;

namespace RetentionTool
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/assets/js/"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/assets/js/vendor/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/assets/scss/bootstrap/"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Scripts/assets/css/bootstrap.css", "~/Scripts/assets/css/bootstrap.min.css","~/Scripts/assets/css/cs-skin-elastic.css",
                      "~/Scripts/assets/css/font-awesome.min.css", "~/Scripts/assets/css/normalize.css","~/Scripts/assets/css/style.css"));

            
        }
    }
}
