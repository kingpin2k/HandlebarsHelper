using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlebarsHelper
{
    public interface ITemplateNamer
    {
        string GenerateName(string filePath, string bundlePath);
    }
}
