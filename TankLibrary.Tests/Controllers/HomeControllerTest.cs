using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TankLibrary;
using TankLibrary.Controllers;
using TankLibrary.Domain.Abstract;
using Moq;
using TankLibrary.Domain.Entities;
using TankLibrary.Models;
using System.Web;
using System.IO;
using System.Resources;
using TankLibrary.HtmlHelpers;

namespace TankLibrary.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        //Menu(): all default, selected stage is null, abbreviationsMenu = false
        [TestMethod]
        public void Menu1()
        {
            // Index(null)
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();

            // Arrange
            NavController controller = new NavController(mock.Object);

            // Act
            PartialViewResult result = controller.Menu();
            IEnumerable<KeyValuePair<string, int>> stages = (IEnumerable<KeyValuePair<string, int>>)result.Model;

            // Assert
            Assert.AreEqual(4, stages.Count());
            Assert.AreEqual("World War I", stages.ElementAt(0).Key);
            Assert.IsNull(result.ViewBag.SelectedStage);
        }

        //Menu(): all default, selectedStage=2, abbreviationsMenu = true
        [TestMethod]
        public void Menu2()
        {
            // Index(null)
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();

            // Arrange
            NavController controller = new NavController(mock.Object);

            // Act
            PartialViewResult result = controller.Menu(2, true);
            IEnumerable<KeyValuePair<string, int>> stages = (IEnumerable<KeyValuePair<string, int>>)result.Model;

            // Assert
            Assert.AreEqual(4, stages.Count());
            Assert.AreEqual("WWI", stages.ElementAt(0).Key);
            Assert.AreEqual(2, result.ViewBag.SelectedStage);
        }

        // Current page is in the middle of all page links on mobile screen
        [TestMethod]
        public void PageLinks1()
        {
            // Arrange
            HtmlHelper htmlHemlper = null;

            // Arrange
            PagingInfo pagingInfo = new PagingInfo
            {
                TotalItems = 27,
                ItemsPerPage = 2,
                CurrentPage = 2,
                CurrentSelIndex = 0
            };

            // Arrange
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = htmlHemlper.PageLinks(pagingInfo, pageUrlDelegate, 3);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1""><</a>" +
                @"<a class=""btn btn-default"" href=""Page1"">1</a>" +
                @"<span class=""disabled btn btn-default btn-primary selected"">2</span>" +
                @"<a class=""btn btn-default"" href=""Page3"">3</a>" +
                @"<a class=""btn btn-default"" href=""Page3"">></a>",
                result.ToString());
        }

        // Current page is the first one of all page links on mobile screen
        [TestMethod]
        public void PageLinks2()
        {
            // Arrange
            HtmlHelper htmlHemlper = null;

            // Arrange
            PagingInfo pagingInfo = new PagingInfo
            {
                TotalItems = 27,
                ItemsPerPage = 2,
                CurrentPage = 1,
                CurrentSelIndex = 0
            };

            // Arrange
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = htmlHemlper.PageLinks(pagingInfo, pageUrlDelegate, 3);

            // Assert
            Assert.AreEqual(@"<span class=""disabled btn btn-default""><</span>" +
                @"<span class=""disabled btn btn-default btn-primary selected"">1</span>" +
                @"<a class=""btn btn-default"" href=""Page2"">2</a>" +
                @"<a class=""btn btn-default"" href=""Page3"">3</a>" +
                @"<a class=""btn btn-default"" href=""Page2"">></a>",
                result.ToString());
        }

        // Current page is the last one of all page links on desktop screen
        [TestMethod]
        public void PageLinks3()
        {
            // Arrange
            HtmlHelper htmlHemlper = null;

            // Arrange
            PagingInfo pagingInfo = new PagingInfo
            {
                TotalItems = 27,
                ItemsPerPage = 2,
                CurrentPage = 14,
                CurrentSelIndex = 0
            };

            // Arrange
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = htmlHemlper.PageLinks(pagingInfo, pageUrlDelegate, 8);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page13"">prev</a>" +
                @"<a class=""btn btn-default"" href=""Page7"">7</a>" +
                @"<a class=""btn btn-default"" href=""Page8"">8</a>" +
                @"<a class=""btn btn-default"" href=""Page9"">9</a>" +
                @"<a class=""btn btn-default"" href=""Page10"">10</a>" +
                @"<a class=""btn btn-default"" href=""Page11"">11</a>" +
                @"<a class=""btn btn-default"" href=""Page12"">12</a>" +
                @"<a class=""btn btn-default"" href=""Page13"">13</a>" +
                @"<span class=""disabled btn btn-default btn-primary selected"">14</span>" +
                @"<span class=""disabled btn btn-default"">next</span>",
                result.ToString());
        }

        // Index(null): all default, stage filter is all, page = 1, selindex = 0
        [TestMethod]
        public void Index1()
        {
            // Index(null)
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Name ="A1", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A2", Stage=2, Type = "Main Battle Field" },
                new Tank { Name ="A3", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A4", Stage=4, Type = "Main Battle Field" },
                new Tank { Name ="A11", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A21", Stage=2, Type = "Main Battle Field" },
                new Tank { Name ="A31", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A41", Stage=4, Type = "Main Battle Field" },
                new Tank { Name ="A12", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A22", Stage=2, Type = "Main Battle Field" },
                new Tank { Name ="A32", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A42", Stage=3, Type = "Main Battle Field" }

            });

            // Arrange 
            HomeController controller = new HomeController(mock.Object);

            // Act
            TankListViewModel result = (TankListViewModel)controller.Index(null).Model;

            // Assert
            Assert.AreEqual(12, result.PageInfo.TotalItems);
            Assert.AreEqual(8, result.PageInfo.ItemsPerPage);
            Assert.AreEqual(1, result.PageInfo.CurrentPage);
            Assert.AreEqual(0, result.PageInfo.CurrentSelIndex);
            Assert.AreEqual(2, result.PageInfo.TotalPages);
            Assert.IsNull(result.CurrentStage);
            Assert.AreEqual(8, result.Tanks.Count());
            Assert.AreEqual("A41", result.Tanks.ElementAt(7).Name);
        }

        // Index(null, 2, 1): with page = 2, selindex = 1
        [TestMethod]
        public void Index2()
        {
            // Index(null, 2, 1)
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Name ="A1", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A2", Stage=2, Type = "Main Battle Field" },
                new Tank { Name ="A3", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A4", Stage=4, Type = "Main Battle Field" },
                new Tank { Name ="A11", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A21", Stage=2, Type = "Main Battle Field" },
                new Tank { Name ="A31", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A41", Stage=4, Type = "Main Battle Field" },
                new Tank { Name ="A12", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A22", Stage=2, Type = "Main Battle Field" },
                new Tank { Name ="A32", Stage=1, Type = "Main Battle Field" },
                new Tank { Name ="A42", Stage=3, Type = "Main Battle Field" }

            });

            // Arrange 
            HomeController controller = new HomeController(mock.Object);

            // Act
            TankListViewModel result = (TankListViewModel)controller.Index(null, 2, 1).Model;

            // Assert
            Assert.AreEqual(12, result.PageInfo.TotalItems);
            Assert.AreEqual(8, result.PageInfo.ItemsPerPage);
            Assert.AreEqual(2, result.PageInfo.CurrentPage);
            Assert.AreEqual(1, result.PageInfo.CurrentSelIndex);
            Assert.AreEqual(2, result.PageInfo.TotalPages);
            Assert.IsNull(result.CurrentStage);
            Assert.AreEqual(4, result.Tanks.Count());
            Assert.AreEqual("A32", result.Tanks.ElementAt(2).Name);
        }

        // Index(1, 1, 3): with a stage filter of 1, page = 1, selindex = 3
        [TestMethod]
        public void Index3()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Id = 11, Name ="A1", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 21, Name ="A2", Stage=2, Type = "Main Battle Field" },
                new Tank { Id = 31, Name ="A3", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 41, Name ="A4", Stage=4, Type = "Main Battle Field" },
                new Tank { Id = 51, Name ="A11", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 61, Name ="A21", Stage=2, Type = "Main Battle Field" },
                new Tank { Id = 71, Name ="A31", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 81, Name ="A41", Stage=4, Type = "Main Battle Field" },
                new Tank { Id = 91, Name ="A12", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 101, Name ="A22", Stage=2, Type = "Main Battle Field" },
                new Tank { Id = 111, Name ="A32", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 121, Name ="A42", Stage=3, Type = "Main Battle Field" }

            });

            // Arrange 
            HomeController controller = new HomeController(mock.Object);

            // Act
            TankListViewModel result = (TankListViewModel)controller.Index(1, 1, 3).Model;

            // Assert
            Assert.AreEqual(6, result.PageInfo.TotalItems);
            Assert.AreEqual(8, result.PageInfo.ItemsPerPage);
            Assert.AreEqual(1, result.PageInfo.CurrentPage);
            Assert.AreEqual(3, result.PageInfo.CurrentSelIndex);
            Assert.AreEqual(1, result.PageInfo.TotalPages);
            Assert.AreEqual(1, result.CurrentStage);
            Assert.AreEqual(6, result.Tanks.Count());
            Assert.AreEqual("A11", result.Tanks.ElementAt(2).Name);
        }

        // Index(1, 1, 4, 121): after add a new tank with Id = 121, stage filter will change by new tank, and focus to it
        [TestMethod]
        public void Index4()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Id = 11, Name ="A1", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 21, Name ="A2", Stage=2, Type = "Main Battle Field" },
                new Tank { Id = 31, Name ="A3", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 41, Name ="A4", Stage=4, Type = "Main Battle Field" },
                new Tank { Id = 51, Name ="A11", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 61, Name ="A21", Stage=2, Type = "Main Battle Field" },
                new Tank { Id = 71, Name ="A31", Stage=3, Type = "Main Battle Field" },
                new Tank { Id = 81, Name ="A41", Stage=4, Type = "Main Battle Field" },
                new Tank { Id = 91, Name ="A12", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 101, Name ="A22", Stage=2, Type = "Main Battle Field" },
                new Tank { Id = 111, Name ="A32", Stage=1, Type = "Main Battle Field" },
                new Tank { Id = 121, Name ="A42", Stage=3, Type = "Main Battle Field" }

            });

            // Arrange 
            HomeController controller = new HomeController(mock.Object);

            // Act
            TankListViewModel result = (TankListViewModel)controller.Index(1, 1, 4, 121).Model;

            // Assert
            Assert.AreEqual(2, result.PageInfo.TotalItems);
            Assert.AreEqual(8, result.PageInfo.ItemsPerPage);
            Assert.AreEqual(1, result.PageInfo.CurrentPage);
            Assert.AreEqual(1, result.PageInfo.CurrentSelIndex);
            Assert.AreEqual(1, result.PageInfo.TotalPages);
            Assert.AreEqual(3, result.CurrentStage);
            Assert.AreEqual(2, result.Tanks.Count());
            Assert.AreEqual("A31", result.Tanks.ElementAt(0).Name);
        }

        [TestMethod]
        public void ImgAndDesc()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Id = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" },
                new Tank { Id = 2, Name = "A2", ImageFile = "testFile2", ImagePath = "testPath2/", Description = "Desc for A2" }
            });

            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            Tank result = (Tank)((PartialViewResult)controller.ImgAndDesc(2)).Model;

            // Assert
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Desc for A2", result.Description);
        }

        [TestMethod]
        public void Detail()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Id = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" },
                new Tank { Id = 2, Name = "A2", ImageFile = "testFile2", ImagePath = "testPath2/", Description = "Desc for A2" }
            });

            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = (ViewResult)controller.Detail(2, 3, 4, 5);

            // Assert
            Assert.AreEqual(2, ((Tank)result.Model).Id);
            Assert.AreEqual("Desc for A2", ((Tank)result.Model).Description);
            Assert.AreEqual(3, result.ViewBag.CurrentStage);
            Assert.AreEqual(4, result.ViewBag.CurrentPage);
            Assert.AreEqual(5, result.ViewBag.CurrentIndex);
        }

        [TestMethod]
        public void Add()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();

            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = (ViewResult)controller.Edit(-1, 3, 4, 5);

            // Assert
            Assert.AreEqual(0, ((Tank)result.Model).Id);
            Assert.AreEqual(3, ((Tank)result.Model).Stage);
            Assert.AreEqual(3, result.ViewBag.CurrentStage);
            Assert.AreEqual(4, result.ViewBag.CurrentPage);
            Assert.AreEqual(5, result.ViewBag.CurrentIndex);
        }

        //HttpGet
        [TestMethod]
        public void Edit1()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Id = 1, Stage=1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" },
                new Tank { Id = 2, Stage=3, Name = "A2", ImageFile = "testFile2", ImagePath = "testPath2/", Description = "Desc for A2" }
            });

            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = (ViewResult)controller.Edit(2, 3, 4, 5);

            // Assert
            Assert.AreEqual(2, ((Tank)result.Model).Id);
            Assert.AreEqual(3, ((Tank)result.Model).Stage);
            Assert.AreEqual(3, result.ViewBag.CurrentStage);
            Assert.AreEqual(4, result.ViewBag.CurrentPage);
            Assert.AreEqual(5, result.ViewBag.CurrentIndex);
        }

        //HttpPost: invalid input
        [TestMethod]
        public void Edit2()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            Tank tank = new Tank { Id = 1, Stage = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" };

            // Arrange
            HomeController controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("error", "error");

            // Act
            ViewResult result = (ViewResult)controller.Edit(tank, 3, 4, 5);

            // Assert - check that the tank hasn't been passed on to the dbcontext
            mock.Verify(m => m.Add(It.IsAny<Tank>()), Times.Never());
            mock.Verify(m => m.Update(It.IsAny<Tank>()), Times.Never());

            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);

            // Assert - check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        //HttpPost: valid input for add
        [TestMethod]
        public void Edit3()
        {
            // Arrange - mock ITankRepository
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Add(It.IsAny<Tank>()))
            .Returns(new Tank { Id = 1, Stage = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" });
            mock.Setup(m => m.Update(It.IsAny<Tank>()));

            // Arrange - tank to submit
            Tank tank = new Tank { Id = -1, Stage = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" };

            // Arrange - mock Request.Files
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpFileCollectionBase> mockFiles = new Mock<HttpFileCollectionBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            string sFileName = "a.jpg";
            var fileKeys = new List<string>() { sFileName };

            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);

            mockRequest.Setup(m => m.Files).Returns(mockFiles.Object);

            mockFiles.Setup(m => m.Count).Returns(1);
            mockFiles.Setup(m => m.GetEnumerator()).Returns(fileKeys.GetEnumerator());
            mockFiles.Setup(m => m[sFileName]).Returns(mockFile.Object);

            mockFile.Setup(m => m.FileName).Returns(sFileName);
            mockFile.Setup(m => m.ContentLength).Returns(0);

            // Arrange
            HomeController controller = new HomeController(mock.Object);
            controller.ControllerContext = new ControllerContext(mockContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            ActionResult result = (ActionResult)controller.Edit(tank, 3, 4, 5);

            // Assert - check that the tank has been passed on to the dbcontext once
            mock.Verify(m => m.Add(It.IsAny<Tank>()), Times.Once());
            mock.Verify(m => m.Update(It.IsAny<Tank>()), Times.Never());

            // Assert - check that the mothod is returning the RedirectToRouteResult
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        //HttpPost: valid input for eidt
        [TestMethod]
        public void Edit4()
        {
            // Arrange - tank to submit
            Tank tank = new Tank { Id = 1, Stage = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" };

            // Arrange - mock ITankRepository
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Add(It.IsAny<Tank>()))
            .Returns(tank);
            mock.Setup(m => m.Update(It.IsAny<Tank>()));

            // Arrange - mock Request.Files
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpFileCollectionBase> mockFiles = new Mock<HttpFileCollectionBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            var sFileName = "a.jpg";
            var fileKeys = new List<string>() { sFileName };

            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);

            mockRequest.Setup(m => m.Files).Returns(mockFiles.Object);

            mockFiles.Setup(m => m.Count).Returns(1);
            mockFiles.Setup(m => m.GetEnumerator()).Returns(fileKeys.GetEnumerator());
            mockFiles.Setup(m => m[sFileName]).Returns(mockFile.Object);

            mockFile.Setup(m => m.FileName).Returns(sFileName);
            mockFile.Setup(m => m.ContentLength).Returns(0);

            // Arrange
            HomeController controller = new HomeController(mock.Object);
            controller.ControllerContext = new ControllerContext(mockContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            ActionResult result = (ActionResult)controller.Edit(tank, 1, 4, 5);

            // Assert - check that the tank has been passed on to the dbcontext once
            mock.Verify(m => m.Add(It.IsAny<Tank>()), Times.Never());
            mock.Verify(m => m.Update(It.IsAny<Tank>()), Times.Once());

            // Assert - check that the mothod is returning the RedirectToRouteResult
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        //HttpPost: valid file size for eidt
        [TestMethod]
        public void Edit5()
        {
            // Arrange - tank to submit
            Tank tank = new Tank { Id = 1, Stage = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" };

            // Arrange - mock ITankRepository
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Add(It.IsAny<Tank>()))
            .Returns(tank);
            mock.Setup(m => m.Update(It.IsAny<Tank>()));

            // Arrange - mock Request.Files
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpFileCollectionBase> mockFiles = new Mock<HttpFileCollectionBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            string sFileName = "a.jpg";
            var fileKeys = new List<string>() { sFileName };

            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);

            mockRequest.Setup(m => m.Files).Returns(mockFiles.Object);

            mockFiles.Setup(m => m.Count).Returns(1);
            mockFiles.Setup(m => m.GetEnumerator()).Returns(fileKeys.GetEnumerator());
            mockFiles.Setup(m => m[sFileName]).Returns(mockFile.Object);

            mockFile.Setup(m => m.FileName).Returns(sFileName);
            mockFile.Setup(m => m.ContentLength).Returns(5000000);

            // Arrange
            HomeController controller = new HomeController(mock.Object);
            controller.ControllerContext = new ControllerContext(mockContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            ViewResult result = (ViewResult)controller.Edit(tank, 1, 4, 5);

            // Assert - check that the tank hasn't been passed on to the dbcontext
            mock.Verify(m => m.Add(It.IsAny<Tank>()), Times.Never());
            mock.Verify(m => m.Update(It.IsAny<Tank>()), Times.Never());

            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);

            // Assert - check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        //HttpPost: valid file ext for eidt
        [TestMethod]
        public void Edit6()
        {
            // Arrange - tank to submit
            Tank tank = new Tank { Id = 1, Stage = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" };

            // Arrange - mock ITankRepository
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Add(It.IsAny<Tank>()))
            .Returns(tank);
            mock.Setup(m => m.Update(It.IsAny<Tank>()));

            // Arrange - mock Request.Files
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            Mock<HttpFileCollectionBase> mockFiles = new Mock<HttpFileCollectionBase>();
            Mock<HttpPostedFileBase> mockFile = new Mock<HttpPostedFileBase>();
            string sFileName = "a.txt";
            var fileKeys = new List<string>() { sFileName };

            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);

            mockRequest.Setup(m => m.Files).Returns(mockFiles.Object);

            mockFiles.Setup(m => m.Count).Returns(1);
            mockFiles.Setup(m => m.GetEnumerator()).Returns(fileKeys.GetEnumerator());
            mockFiles.Setup(m => m[sFileName]).Returns(mockFile.Object);

            mockFile.Setup(m => m.FileName).Returns(sFileName);
            mockFile.Setup(m => m.ContentLength).Returns(20000);

            // Arrange
            HomeController controller = new HomeController(mock.Object);
            controller.ControllerContext = new ControllerContext(mockContext.Object, new System.Web.Routing.RouteData(), controller);

            // Act
            ViewResult result = (ViewResult)controller.Edit(tank, 1, 4, 5);

            // Assert - check that the tank hasn't been passed on to the dbcontext
            mock.Verify(m => m.Add(It.IsAny<Tank>()), Times.Never());
            mock.Verify(m => m.Update(It.IsAny<Tank>()), Times.Never());

            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);

            // Assert - check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Id = 1, Name = "A1", ImageFile = "testFile", ImagePath = "testPath/", Description = "Desc for A1" },
                new Tank { Id = 2, Name = "A2", ImageFile = "testFile2", ImagePath = "testPath2/", Description = "Desc for A2" }
            });

            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = (ViewResult)controller.Delete(2, 3, 4, 5);

            // Assert
            Assert.AreEqual(2, ((Tank)result.Model).Id);
            Assert.AreEqual("Desc for A2", ((Tank)result.Model).Description);
            Assert.AreEqual(3, result.ViewBag.CurrentStage);
            Assert.AreEqual(4, result.ViewBag.CurrentPage);
            Assert.AreEqual(5, result.ViewBag.CurrentIndex);
        }

        [TestMethod]
        public void DeleteConfirmed()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Delete(2));

            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            ActionResult result = controller.DeleteConfirmed(2, 3, 4, 5);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Name = "A1", Stage = 1, Weight = 50 },
                new Tank { Name = "A2", Stage = 2, Weight = 60 },
                new Tank { Name = "A3", Stage = 3, Weight = 55 },
                new Tank { Name = "A4", Stage = 2, Weight = 48 },
                new Tank { Name = "A5", Stage = 3, Weight = 63 },
                new Tank { Name = "A6", Stage = 4, Weight = 52 },
                new Tank { Name = "A7", Stage = 1, Weight = 50 }
            });
            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("TANK LIBRARY is a simple application created with C#, ASP.NET, MVC5, HTML5, CSS3 and JAVASCRIPT.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            Mock<ITankRepository> mock = new Mock<ITankRepository>();
            mock.Setup(m => m.Tanks).Returns(new Tank[]
            {
                new Tank { Name = "A1", Stage = 1, Weight = 50 },
                new Tank { Name = "A2", Stage = 2, Weight = 60 },
                new Tank { Name = "A3", Stage = 3, Weight = 55 },
                new Tank { Name = "A4", Stage = 2, Weight = 48 },
                new Tank { Name = "A5", Stage = 3, Weight = 63 },
                new Tank { Name = "A6", Stage = 4, Weight = 52 },
                new Tank { Name = "A7", Stage = 1, Weight = 50 }
            });
            // Arrange
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
