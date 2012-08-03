using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Bootstrap.AutoMapper;
using Bootstrap.Extensions.StartupTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Raven.Client;
using Raven.Client.Embedded;
using RavenCms.Controllers;
using RavenCms.ViewModels;

namespace RavenCms.Test.Controllers
{
    [TestClass]
    public class ContentControllerTest : IDisposable
    {
        private readonly IDocumentStore _documentStore;

        public ContentControllerTest()
        {
            _documentStore = new EmbeddableDocumentStore {RunInMemory = true};
            _documentStore.Initialize();

            Bootstrap.Bootstrapper.With.AutoMapper().Start();
        }

        [TestMethod]
        public void Show_DoesNotExist_ReturnsPlaceholderViewModel()
        {
            //Arrange
            var controller = new ContentController {RavenSession = _documentStore.OpenSession()};
            string contentName = "Slogan";

            //Act
            var result = (ViewResult)controller.Show(contentName);
            var model = (ContentViewModel)result.Model;
            
            //Assert
            Assert.AreEqual("Slogan", model.Body);
        }

        [TestMethod]
        public void Show_Exists_ReturnsViewModel()
        {
            //Arrange
            StoreSampleContent();

            var controller = new ContentController { RavenSession = _documentStore.OpenSession() };
            
            //Act
            var result = (ViewResult)controller.Show("Slogan");
            var model = (ContentViewModel)result.Model;

            //Assert
            Assert.AreEqual("A good slogan for your website", model.Body);
        }

        private void StoreSampleContent()
        {
            using (var session = _documentStore.OpenSession())
            {
                var content = new Content.Content {Id = "Slogan", Body = "A good slogan for your website"};
                session.Store(content);
                session.SaveChanges();
            }
        }

        [TestMethod]
        public void Save_IsValid_StoresInDatabase()
        {
            //Arrange
            var controller = new ContentController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new ContentViewModel {Id = "Slogan", Body = "My content view model"};

            //Act
            controller.Save(viewModel);
            controller.RavenSession.SaveChanges();

            //Assert
            using (var session = _documentStore.OpenSession())
            {
                var content = session.Load<Content.Content>("Slogan");
                Assert.AreEqual("Slogan", content.Id);
                Assert.AreEqual("My content view model", content.Body);
            }
        }

        [TestMethod]
        public void Save_IsValid_ReturnsSuccessView()
        {
            //Arrange
            var controller = new ContentController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new ContentViewModel { Id = "Slogan", Body = "My content view model" };

            //Act
            var result = (ViewResult)controller.Save(viewModel);

            //Assert
            result.AssertViewRendered().ForView("Success");
        }

        [TestMethod]
        public void Save_IsNotValid_DoesNotStore()
        {
            //Arrange
            var controller = new ContentController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new ContentViewModel { Id = "Slogan", Body = "My content view model" };
            controller.ModelState.AddModelError("Body", "Body is too long");

            //Act
            var result = controller.Save(viewModel);
            controller.RavenSession.SaveChanges();

            //Assert
            using (var session = _documentStore.OpenSession())
            {
                var content = session.Load<Content.Content>("Slogan");
                Assert.IsNull(content);
            }
        }

        [TestMethod]
        public void Save_IsNotValid_ReturnsFailureView()
        {
            //Arrange
            var controller = new ContentController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new ContentViewModel { Id = "Slogan", Body = "My content view model" };
            controller.ModelState.AddModelError("Body", "Body is too long");

            //Act
            var result = (ViewResult)controller.Save(viewModel);

            //Assert
            result.AssertViewRendered().ForView("Failure");
        }

        public void Dispose()
        {
            _documentStore.Dispose();
        }
    }
    
}
