using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlebarsHelper
{
    public class TemplateNamer : ITemplateNamer
    {
        readonly char[] DirectorySeparator;

        public TemplateNamer()
        {
            this.DirectorySeparator = new char[] { Path.DirectorySeparatorChar };
        }

        public string GenerateName(string filePath, string bundlePath)
        {
            var fileName = Path.GetFileName(filePath);
            var fileNameNoExtension = Path.GetFileNameWithoutExtension(fileName);
            var fileLocation = Path.GetDirectoryName(filePath);
            var relativeFileLocation = fileLocation.Substring(bundlePath.Length);

            var directories = relativeFileLocation.Split(DirectorySeparator, StringSplitOptions.RemoveEmptyEntries);

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
