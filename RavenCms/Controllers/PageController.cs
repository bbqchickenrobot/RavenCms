using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using RavenCms.Content;

namespace RavenCms.Controllers
{
    public class PageController : RavenController
    {
        public ActionResult Show(string url)
        {
            var page = RavenSession.Query<Page>().SingleOrDefault(p => p.Url == url);
            if(page == null)
                return new HttpNotFoundResult();

            var viewModel = Mapper.Map<PageViewModel>(page);
            return View(viewModel);
        }
    }

    public class PageViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Url { get; set; }
    }
}
