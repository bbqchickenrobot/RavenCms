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
using Raven.Client;
using Raven.Client.Embedded;
using RavenCms.Controllers;

namespace RavenCms.Test.Controllers
{
    [TestClass]
    public class ContentControllerTest
    {
        private readonly IDocumentStore _documentStore;

        public ContentControllerTest()
        {
            _documentStore = new EmbeddableDocumentStore {RunInMemory = true};
            _documentStore.Initialize();

            Bootstrap.Bootstrapper.With.AutoMapper().Start();
        }

        [TestMethod]
        public void Show_ReturnsPlaceholderViewModel_DoesNotExist()
        {
            //Arrange
            var controller = new ContentController {RavenSession = _documentStore.OpenSession()};
            string contentName = "Slogan";

            //Act
            var result = controller.Show(contentName);
            var model = (ContentViewModel)result.Model;
            
            //Assert
            Assert.AreEqual("Slogan", model.Body);
        }

        [TestMethod]
        public void Show_ReturnsViewModel_LoadFromDatabase()
        {
            //Arrange
            using (var session = _documentStore.OpenSession())
            {
                var content = new Content.Content {Id = "Slogan", Body = "A good slogan for your website"};
                session.Store(content);
                session.SaveChanges();
            }

            var controller = new ContentController { RavenSession = _documentStore.OpenSession() };
            
            //Act
            var result = controller.Show("Slogan");
            var model = (ContentViewModel)result.Model;

            //Assert
            Assert.AreEqual("A good slogan for your website", model.Body);
        }

        [TestMethod]
        public void Save_StoresInDatabase_IsValid()
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
        public void Save_ReturnsSuccessView_IsValid()
        {
            //Arrange
            var controller = new ContentController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new ContentViewModel { Id = "Slogan", Body = "My content view model" };

            //Act
            var result = controller.Save(viewModel);

            //Assert
            Assert.AreEqual("Success", result.ViewName);
        }

        [TestMethod]
        public void Save_DoesNotStore_IsNotValid()
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
        public void Save_ReturnsFailureView_IsNotValid()
        {
            //Arrange
            var controller = new ContentController { RavenSession = _documentStore.OpenSession() };
            var viewModel = new ContentViewModel { Id = "Slogan", Body = "My content view model" };
            controller.ModelState.AddModelError("Body", "Body is too long");

            //Act
            var result = controller.Save(viewModel);

            //Assert
            Assert.AreEqual("Failure", result.ViewName);
        }
    }
    
}
