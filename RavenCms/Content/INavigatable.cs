using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenCms.Content
{
    interface INavigatable
    {
        string Path { get; set; }
        string Title { get; set; }
    }
}
