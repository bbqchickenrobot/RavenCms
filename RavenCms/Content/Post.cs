using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenCms.Content
{
    class Post : IContent, INavigatable
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
    }
}
