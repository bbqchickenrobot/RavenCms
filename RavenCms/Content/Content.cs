using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenCms.Content
{
    public class Content : IContent
    {
        public string Id { get; set; }
        public string Body { get; set; }
    }
}
