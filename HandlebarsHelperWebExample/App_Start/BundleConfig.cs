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
                                    "~/Scripts/handlebars-v2.0.0.js").Include(
                                    "~/Scripts/ember.js").Include(
                                    "~/Scripts/app.js"));

            bundles.Add(new Bundle("~/bundles/templates", new HandlebarsTransformer())
                .IncludeDirectory("~/Scripts/templates", "*.hbs", true));

            bundles.Add(new StyleBundle("~/bundles/styles")
                .Include("~/Content/styles.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}