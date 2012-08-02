using System;
using System.Web.Mvc;
using Bootstrap.AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;
using RavenCms.Content;
using RavenCms.Controllers;
using RavenCms.ViewModels;

namespace RavenCms.Test.Controllers
{
    [TestClass]
    public class PageControllerTest : IDisposable
    {
        private readonly IDocumentStore _documentStore;

        public PageControllerTest()
        {
            _documentStore = new EmbeddableDocumentStore { RunInMemory = true };
            _documentStore.Initialize();

            Bootstrap.Bootstrapper.With.AutoMapper().Start();
        }

        [TestMethod]
        public void Show_ReturnsView_IfExists()
        {
            //Arrange
            using (var session = _documentStore.OpenSession())
            {
                session.Store(new Page {Id = 1, Body = "My page", Url = "about-us/team/managment" });
                session.SaveChanges();
            }

            var controller = new PageController { RavenSession = _documentStore.OpenSession() };
            
            //Act
            var result = (ViewResult)controller.Show("about-us/team/managment");
            var model = (PageViewModel)result.Model;

            //Assert
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("My page", model.Body);
            Assert.AreEqual("about-us/team/managment", model.Url);
        }

        [TestMethod]
        public void Show_ReturnsNotFoundView_IfNotExists()
        {
            //Arrange
            var controller = new PageController { RavenSession = _documentStore.OpenSession() };

            //Act
            var result = (HttpNotFoundResult)controller.Show("about-us/team/managment");

            //Assert
            Assert.AreEqual(404, result.StatusCode);
        }




        [TestMethod]
        public void Save_StoresInDatabase_IsValid()
        {
            //Arrange
            var controller = new PageController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new PageViewModel { Id = 1, Body = "My page", Url = "about-us/team/managment" };

            //Act
            controller.Save(viewModel);
            controller.RavenSession.SaveChanges();

            //Assert
            using (var session = _documentStore.OpenSession())
            {
                var page = session.Load<Page>(1);
                Assert.AreEqual("My page", page.Body);
                Assert.AreEqual("about-us/team/managment", page.Url);
            }
        }

        [TestMethod]
        public void Save_ReturnsSuccessView_IsValid()
        {
            //Arrange
            var controller = new PageController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new PageViewModel { Id = 1, Body = "My page", Url = "about-us/team/managment" };

            //Act
            var result = (PartialViewResult)controller.Save(viewModel);

            //Assert
            Assert.AreEqual("Success", result.ViewName);
        }

        [TestMethod]
        public void Save_DoesNotStore_IsNotValid()
        {
            //Arrange
            var controller = new PageController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new PageViewModel { Id = 1, Body = "My page", Url = "about-us/team/managment" };
            controller.ModelState.AddModelError("Body", "Body is too long");

            //Act
            var result = controller.Save(viewModel);
            controller.RavenSession.SaveChanges();

            //Assert
            using (var session = _documentStore.OpenSession())
            {
                var content = session.Load<Page>(1);
                Assert.IsNull(content);
            }
        }

        [TestMethod]
        public void Save_ReturnsFailureView_IsNotValid()
        {
            //Arrange
            var controller = new PageController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new PageViewModel { Id = 1, Body = "My page", Url = "about-us/team/managment" };
            controller.ModelState.AddModelError("Body", "Body is too long");

            //Act
            var result = controller.Save(viewModel);

            //Assert
            Assert.AreEqual("Failure", result.ViewName);
        }


        public void Dispose()
        {
            _documentStore.Dispose();
        }
    }
}
