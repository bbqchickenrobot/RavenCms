using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;

namespace RavenCms.Controllers
{
    public class ContentController : RavenController
    {
        [ChildActionOnly]
        public virtual PartialViewResult Show(string contentName)
        {
            var content = RavenSession.Load<Content.Content>(contentName);
            var viewModel = content == null ? new ContentViewModel{Body = contentName} : Mapper.Map<ContentViewModel>(content);
            return PartialView(viewModel);
        }

        public virtual PartialViewResult Save(ContentViewModel viewModel)
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

    public class ContentViewModel
    {
        public string Id { get; set; }
        public string Body { get; set; }
    }
}
