using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlebarsHelper
{
    public class FileToolkit
    {
        public static string PathDifference(string filePath, string directory)
        {
            var name = Path.GetFileName(filePath);
            var relPath = filePath.Substring(directory.Length);
            return relPath.Substring(0, relPath.Length - name.Length);
        }
    }
}
