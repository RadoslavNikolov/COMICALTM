using System.Web.Optimization;

namespace Contests.App
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.11.4.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                        "~/Scripts/jquery.signalR-2.2.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.js",
                     "~/Scripts/respond.js",
                     "~/Scripts/moment-with-locales.js",
                     "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/style.css",
                      "~/Content/pager.css",
                      "~/Content/lightbox.css",
                      "~/Content/bootstrap-datetimepicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/Scripts/CustomScripts/new-contest-options.js",
                "~/Scripts/lightbox.js"));

            bundles.Add(new StyleBundle("~/content/toastr", "http://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css")
                .Include("~/Content/toastr.css"));

            bundles.Add(new ScriptBundle("~/bundles/toastr", "http://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js")
                            .Include("~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/multiselect").Include(
                "~/Scripts/jquery.quicksearch.js",
                "~/Scripts/jquery.multi-select.js",
                "~/Scripts/CustomScripts/multiselect.js"));

            bundles.Add(new StyleBundle("~/Content/multiselect").Include(
                      "~/Content/multi-select.css"));
        }
    }
}
