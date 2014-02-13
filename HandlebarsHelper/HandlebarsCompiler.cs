using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Yahoo.Yui.Compressor;

namespace HandlebarsHelper
{
    public class HandlebarsCompiler:IDisposable
    {
        Jurassic.ScriptEngine Engine;
        JavaScriptCompressor Compressor;

        public HandlebarsCompiler()
        {
            Engine = new Jurassic.ScriptEngine();
            var ass = typeof(HandlebarsCompiler).Assembly;
            Engine.Execute("var exports = {};");
            Engine.Execute(GetEmbeddedResource("HandlebarsHelper.Scripts.handlebars-v1.3.0.js", ass));
            Engine.Execute(GetEmbeddedResource("HandlebarsHelper.Scripts.ember-template-compiler.js", ass));
            Engine.Execute("var precompile = exports.precompile;");
            Compressor = new JavaScriptCompressor();
        }

        public string Precompile(string template, bool compress)
        {
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

        public void Dispose()
        {

        }
    }
}
