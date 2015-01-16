using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yahoo.Yui.Compressor;

namespace HandlebarsHelper
{
    public class HandlebarsCompiler
    {
        Jurassic.ScriptEngine Engine;
        JavaScriptCompressor Compressor;

        public HandlebarsCompiler()
        {
            Engine = new Jurassic.ScriptEngine();
            var ass = typeof(HandlebarsCompiler).Assembly;
            Engine.Execute("var exports = {};");
            Engine.Execute("var module = {};");
            Engine.Execute(GetEmbeddedResource("HandlebarsHelper.Scripts.handlebars-v2.0.0.js", ass));
            Engine.Execute(GetEmbeddedResource("HandlebarsHelper.Scripts.ember-template-compiler.js", ass));
            Engine.Execute("var precompile = exports.precompile;");
            Compressor = new JavaScriptCompressor();
        }

        public string Precompile(string template, bool compress)
        {
            template = MinifyHtml(template);

            // \r\n is breaking css, it's worthless to keep it in at this point anyway
            template = template.Replace(Environment.NewLine, " ");

            var compiled = Engine.CallGlobalFunction<string>("precompile", template, false);
            if (compress)
            {
                compiled = Compressor.Compress(compiled);
            }
            return compiled;
        }

        public string GetEmbeddedResource(string resource, Assembly ass)
        {
            using (var reader = new StreamReader(ass.GetManifestResourceStream(resource)))
            {
                return reader.ReadToEnd();
            }
        }

        public static string MinifyHtml(string htmlContents)
        {
            // Replace line comments
            htmlContents = Regex.Replace(htmlContents, @"// (.*?)\r?\n", "", RegexOptions.Singleline);

            // Replace spaces between quotes
            htmlContents = Regex.Replace(htmlContents, @"\s+", " ");

            // Replace line breaks
            htmlContents = Regex.Replace(htmlContents, @"\s*\n\s*", "\n");

            // Replace spaces between brackets
            htmlContents = Regex.Replace(htmlContents, @"\s*\>\s*\<\s*", "><");

            // Replace comments
            htmlContents = Regex.Replace(htmlContents, @"<!--(?!\[)(.*?)-->", "");

            return htmlContents.Trim();
        }
    }
}
