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
            var root = context.HttpContext.Server.MapPath(context.BundleVirtualPath);
            var templates = new Dictionary<string, string>();
            foreach (var bundleFile in response.Files)
            {

                var filePath = context.HttpContext.Request.MapPath(bundleFile.VirtualFile.VirtualPath);
                var templateName = namer.GenerateName(filePath, root);
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

    }
}
