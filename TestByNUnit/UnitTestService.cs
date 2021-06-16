using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SearchYourTrack.Controllers;
using SearchYourTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestByNUnit
{
    public class UnitTestService
    {
        private ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var mock = new Mock<ILogger<HomeController>>();
            _logger = mock.Object;
            _homeController = new HomeController(_logger, _configuration);
        }

        /// <summary>
        /// Test for Model Data in returned ViewResult against valid inputs
        /// </summary>
        [TestCase("Bob", "Pattern")]
        [TestCase("Bob", "Artists")]
        [TestCase("Alice", "Pattern")]
        [TestCase("Alice", "Artists")]
        public void TestForViewResultAndValidSearchResultData(string SearchInput, string SearchByOption)
        {
            ViewResult viewResult = new ViewResult();
            SongSearchModel songSearchModel = new SongSearchModel();
            songSearchModel.SearchInput = SearchInput;
            songSearchModel.SearchByOption = SearchByOption;
            var result = _homeController.FetchSongData(songSearchModel) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

            if (result != null)
            {
                Assert.IsInstanceOf(typeof(SongSearchModel), result.Model);
                SongSearchModel dataModel = result.Model as SongSearchModel;
                Assert.IsNotNull(dataModel);
                Assert.IsNotNull(dataModel.Root);
                Assert.IsNotNull(dataModel.Root.NSArray);
                Assert.IsNotNull(dataModel.Root.NSArray.Song);
                Assert.IsTrue(dataModel.Root.NSArray.Song.Count > 0);
            }
        }

        /// <summary>
        /// Test for Model Data in returned ViewResult against invalid inputs
        /// </summary>
        [TestCase("noxyzdata", "Pattern")]
        [TestCase("nopqrdata", "Artists")]
        public void TestForViewResultAndInValidSearchResultData(string SearchInput, string SearchByOption)
        {
            ViewResult viewResult = new ViewResult();
            SongSearchModel songSearchModel = new SongSearchModel();
            songSearchModel.SearchInput = SearchInput;
            songSearchModel.SearchByOption = SearchByOption;
            var result = _homeController.FetchSongData(songSearchModel) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

            if (result != null)
            {
                Assert.IsInstanceOf(typeof(SongSearchModel), result.Model);
                SongSearchModel dataModel = result.Model as SongSearchModel;
                Assert.IsNotNull(dataModel);
                Assert.IsNotNull(dataModel.Root);
                Assert.IsNull(dataModel.Root.NSArray);
            }
        }

        /// <summary>
        /// Test for Model Data in returned ViewResult against invalid options
        /// </summary>
        [TestCase("Bob", "xyz")]
        [TestCase("Alice", "pqr")]
        public void TestForViewResultAndInValidOptionSearchResultData(string SearchInput, string SearchByOption)
        {
            ViewResult viewResult = new ViewResult();
            SongSearchModel songSearchModel = new SongSearchModel();
            songSearchModel.SearchInput = SearchInput;
            songSearchModel.SearchByOption = SearchByOption;
            var result = _homeController.FetchSongData(songSearchModel) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

            if (result != null)
            {
                Assert.IsInstanceOf(typeof(SongSearchModel), result.Model);
                SongSearchModel dataModel = result.Model as SongSearchModel;
                Assert.IsNotNull(dataModel);
                Assert.IsNotNull(dataModel.Root);
                Assert.IsNotNull(dataModel.Root.NSArray);
                Assert.IsNotNull(dataModel.Root.NSArray.Song);
                Assert.IsTrue(dataModel.Root.NSArray.Song.Count > 0);
            }
        }


        /// <summary>
        /// Test for invalid ModelState of PostBack method in HomeController
        /// </summary>
        [TestCase("Bob", "Artists")]
        [TestCase("Alice", "Pattern")]
        public void InvalidModelState(string SearchInput, string SearchByOption)
        {
            ViewResult viewResult = new ViewResult();
            SongSearchModel songSearchModel = new SongSearchModel();
            _homeController.ModelState.AddModelError("test", "TestError");
            songSearchModel.SearchInput = SearchInput;
            songSearchModel.SearchByOption = SearchByOption;
            var result = _homeController.FetchSongData(songSearchModel) as ViewResult;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");

            if (result != null)
            {
                Assert.IsInstanceOf(typeof(SongSearchModel), result.Model);
                SongSearchModel dataModel = result.Model as SongSearchModel;
                Assert.IsNotNull(dataModel);
                Assert.IsNull(dataModel.Root);
            }
        }

    }

}

