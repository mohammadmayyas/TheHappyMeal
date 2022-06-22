using System.Web.Optimization;

namespace IdentitySample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/vendor/animsition/js/animsition.min.js",
                        "~/Content/vendor/bootstrap/js/popper.js",
                        "~/Content/vendor/select2/select2.min.js",
                        "~/Content/vendor/daterangepicker/moment.min.js",
                        "~/Content/vendor/daterangepicker/daterangepicker.js",
                        "~/Content/vendor/slick/slick.min.js",
                        "~/Content/js/slick-custom.js",
                        "~/Content/vendor/parallax100/parallax100.js",
                        "~/Content/vendor/countdowntime/countdowntime.js",
                        "~/Content/vendor/lightbox2/js/lightbox.min.js",
                        "~/Content/js/map-custom.js",
                        "~/Content/js/main.js"
                        /*"~/Content/Scripts/OrderScript.js"*/));
                        
                        

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/css/main.css",
                      "~/Content/css/util.css",
                      "~/Content/fonts/font-awesome-4.7.0/css/font-awesome.min.css",
                      "~/Content/fonts/themify/themify-icons.css",
                      "~/Content/vendor/animate/animate.css",
                      "~/Content/vendor/css-hamburgers/hamburgers.min.css",
                      "~/Content/vendor/animsition/css/animsition.min.css",
                      "~/Content/vendor/select2/select2.min.css",
                      "~/Content/vendor/daterangepicker/daterangepicker.css",
                      "~/Content/vendor/slick/slick.css",
                      "~/Content/vendor/lightbox2/css/lightbox.min.css"));
        }
    }
}
