using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace HandlebarsHelper
{
    public static class RawTemplateInjector
    {
        public static IHtmlString InjectRawTemplates(string templatePath, string[] templateExtensions)
        {
            return GenerateTemplates(templatePath, templateExtensions, new TemplateNamer());
        }

        public static IHtmlString InjectRawTemplates(string templatePath, string[] templateExtensions, ITemplateNamer templateNamer)
        {
            return GenerateTemplates(templatePath, templateExtensions, templateNamer);
        }

        private static IHtmlString GenerateTemplates(string templatePath, string[] templateExtensions, ITemplateNamer templateNamer)
        {
            templatePath = HttpContext.Current.Request.MapPath(templatePath);
            var files = FindFiles(templatePath, templatePath, templateExtensions);

            return BuildTemplates(templatePath, files, new TemplateNamer());
        }

        private static IHtmlString BuildTemplates(string templatePath, List<string> files, ITemplateNamer templateNamer)
        {
            var templates = new StringBuilder();
            foreach (var template in files)
            {
                var templateName = templateNamer.GenerateName(template, templatePath);
                templates.AppendFormat("<script type='text/x-handlebars' data-template-name='{0}'>\n", templateName);
                templates.AppendLine(File.ReadAllText(template));
                templates.AppendLine("</script>");
            }
            return new MvcHtmlString(templates.ToString());
        }

        private static List<string> FindFiles(string searchPath, string templatePath, string[] templateExtensions)
        {
            var files = new List<string>();

            foreach (var dir in Directory.GetDirectories(searchPath))
            {
                files.AddRange(FindFiles(dir, templatePath, templateExtensions));
            }

            files.AddRange(templateExtensions.SelectMany(extension => Directory.GetFiles(searchPath, extension)));

            return files;
        }
    }
}
