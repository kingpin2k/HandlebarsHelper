using MsieJavaScriptEngine;
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
    public class HandlebarsCompiler
    {
        MsieJsEngine Engine;
        JavaScriptCompressor Compressor;

        public HandlebarsCompiler()
        {
            Engine = new MsieJsEngine(true, true);
            Engine.SetVariableValue("exports", new object());
            Engine.ExecuteResource("HandlebarsHelper.Scripts.handlebars-v1.2.0.js", typeof(HandlebarsCompiler));
            Engine.ExecuteResource("HandlebarsHelper.Scripts.ember-template-compiler.js", typeof(HandlebarsCompiler));
            Engine.Execute("var precompile = exports.precompile;");
            Compressor = new JavaScriptCompressor();
        }

        public string Precompile(string template, bool compress)
        {
            var compiled = Engine.CallFunction<string>("precompile", template, false);
            if (compress)
            {
                compiled = Compressor.Compress(compiled);
            }
            return compiled;
        }
    }
}
