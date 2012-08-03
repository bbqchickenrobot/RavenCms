using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using RavenCms.ViewModels;

namespace RavenCms.Controllers
{
    public class ContentController : RavenController
    {
        [ChildActionOnly]
        public virtual ActionResult Show(string contentName)
        {
            var content = RavenSession.Load<Content.Content>(contentName);
            var viewModel = content == null ? new ContentViewModel{Body = contentName} : Mapper.Map<ContentViewModel>(content);
            return PartialView(viewModel);
        }

        public virtual ActionResult Save(ContentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var content = Mapper.Map<Content.Content>(viewModel);
                RavenSession.Store(content);
                return PartialView("Success");
            }
            return PartialView("Failure");
        }
    }
}
