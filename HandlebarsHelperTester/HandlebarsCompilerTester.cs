using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HandlebarsHelper;

namespace HandlebarsHelperTester
{
    [TestClass]
    public class HandlebarsCompilerTester
    {
        [TestMethod]
        public void TestCompiler()
        {
            //lazy test
            HandlebarsCompiler hc = new HandlebarsCompiler();
            var template = hc.Precompile("asdf {{asdf}}", false);
            Assert.IsNotNull(template);
            Assert.IsTrue(template.IndexOf("asdf") > 0);
        }
    }
}
