using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;

namespace HandlebarsHelper
{
    public class TemplateNamer : ITemplateNamer
    {
        readonly char[] DirectorySeparator;

        public TemplateNamer()
        {
            this.DirectorySeparator = new char[] { Path.DirectorySeparatorChar };
        }

        public string GenerateName(string bundleRelativePath, string fileName)
        {
            var fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);

            var directories = bundleRelativePath.Split(DirectorySeparator, StringSplitOptions.RemoveEmptyEntries);

            var temp = String.Join("/", directories);

            if (!temp.EndsWith(fileNameNoExtension))
            {
                if (!string.IsNullOrEmpty(temp))
                {
                    temp += "/";
                }
                temp += fileNameNoExtension;
            }
            return temp;
        }
    }
}
