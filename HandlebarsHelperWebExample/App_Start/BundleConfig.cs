using HandlebarsHelper;
using System.Web;
using System.Web.Optimization;

namespace HandlebarsHelperWebExample
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                                    "~/Scripts/handlebars-v1.2.0.js").Include(
                                    "~/Scripts/ember.js").Include(
                                    "~/Scripts/app.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/templates", new HandlebarsTransformer())
                .IncludeDirectory("~/Scripts/templates", "*.hbs", true));

            BundleTable.EnableOptimizations = true;
        }
    }
}