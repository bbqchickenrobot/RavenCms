using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using RavenCms.Content;
using RavenCms.ViewModels;

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

        public ActionResult Save(PageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var page = Mapper.Map<Page>(viewModel);
                RavenSession.Store(page);
                return PartialView("Success");
            }
            return PartialView("Failure");
        }
    }
}
