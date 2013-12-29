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
        private HandlebarsCompiler _compiler = null;
        private HandlebarsCompiler Compiler
        {
            get
            {
                if (_compiler == null)
                {
                    _compiler = new HandlebarsCompiler();
                }
                return _compiler;
            }
        }

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
            var root = context.HttpContext.Server.MapPath(context.BundleVirtualPath);
            var templates = new Dictionary<string, string>();
            foreach (var bundleFile in response.Files)
            {

                var filePath = context.HttpContext.Server.MapPath(bundleFile.VirtualFile.VirtualPath);
                var templateName = namer.GenerateName(filePath, root);
                var template = File.ReadAllText(filePath);
                var compiled = Compiler.Precompile(template, false);

                templates[templateName] = compiled;
            }
            StringBuilder javascript = new StringBuilder();
            foreach (var templateName in templates.Keys)
            {
                javascript.AppendFormat("Ember.Templates['{0}']=", templateName);
                javascript.AppendFormat("Ember.Handlebars.template({0});", templates[templateName]);
            }

            var Compressor = new JavaScriptCompressor();

            response.ContentType = "text/javascript";
            response.Cacheability = HttpCacheability.Public;
            response.Content = Compressor.Compress(javascript.ToString());
        }

    }
}
