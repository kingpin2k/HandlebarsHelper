using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;
using Yahoo.Yui.Compressor;

namespace HandlebarsHelper
{
    public class HandlebarsTransformer : IBundleTransform
    {
        ITemplateNamer namer;

        public HandlebarsTransformer()
        {
            namer = new TemplateNamer();
        }

        public HandlebarsTransformer(ITemplateNamer templateNamer)
        {
            this.namer = templateNamer;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            var compiler = new HandlebarsCompiler();
            var templates = new Dictionary<string, string>();
            var server = context.HttpContext.Server;
         
            foreach (var bundleFile in response.Files)
            {
                var filePath = server.MapPath(bundleFile.VirtualFile.VirtualPath);
                var bundleRelativePath = GetRelativePath(server, bundleFile, filePath);
                var templateName = namer.GenerateName(bundleRelativePath, bundleFile.VirtualFile.Name);
                var template = File.ReadAllText(filePath);
                var compiled = compiler.Precompile(template, false);

                templates[templateName] = compiled;
            }
            StringBuilder javascript = new StringBuilder();
            foreach (var templateName in templates.Keys)
            {
                javascript.AppendFormat("Ember.TEMPLATES['{0}']=", templateName);
                javascript.AppendFormat("Ember.Handlebars.template({0});", templates[templateName]);
            }

            var Compressor = new JavaScriptCompressor();
            var compressed = Compressor.Compress(javascript.ToString());

            response.ContentType = "text/javascript";
            response.Cacheability = HttpCacheability.Public;
            response.Content = javascript.ToString();
        }

        private string GetRelativePath(HttpServerUtilityBase server, BundleFile bundleFile, string filePath)
        {
            var relativeBundlePath = bundleFile.IncludedVirtualPath.Remove(bundleFile.IncludedVirtualPath.IndexOf(@"\"));
            var bundlePath = server.MapPath(relativeBundlePath);
            return FileToolkit.PathDifference(filePath, bundlePath);
        }

    }
}
