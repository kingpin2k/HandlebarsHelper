using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;

namespace HandlebarsHelper
{
    public interface ITemplateNamer
    {
        string GenerateName(string bundleRelativePath, string fileName);
    }
}
