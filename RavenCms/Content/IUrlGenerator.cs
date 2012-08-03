using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenCms.Content
{
    public interface IUrlGenerator
    {
        string GenerateUrl(string location);
    }
}
