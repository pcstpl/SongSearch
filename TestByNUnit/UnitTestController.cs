using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SearchYourTrack.Controllers;
using SearchYourTrack.Models;

namespace TestByNUnit
{
    public class UnitTestController
    {
        private ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        private HomeController _homeController;

        [SetUp]
        public void Setup()
        {
            _configuration= new ConfigurationBuilder()
                .AddJsonFile("appsettings.json" ,optional:false,reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();
            var mock = new Mock<ILogger<HomeController>>();
            _logger = mock.Object;
            _homeController = new HomeController(_logger,_configuration);
        }

        /// <summary>
        /// Test for returned ViewResult after PostBack
        /// </summary>
        [TestCase("Bob", "Pattern")]
        [TestCase("Bob", "Artists")]
        [TestCase("Alice", "Pattern")]
        [TestCase("Alice", "Artists")]
        public void TestForValidViewResult(string SearchInput, string SearchByOption)
        {
            SongSearchModel songSearchModel = new SongSearchModel();
            songSearchModel.SearchInput = SearchInput;
            songSearchModel.SearchByOption = SearchByOption;
            var result = _homeController.FetchSongData(songSearchModel) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName=="Index");
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

        /// <summary>
        /// Test case for max search string input length
        /// </summary>
        [TestCase("Search string of length beyond limit of fifty characters", "Artists")]
        [TestCase("Search string of length beyond limit of fifty characters", "Pattern")]
        public void BeyondMaxLengthSearchString(string SearchInput, string SearchByOption)
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
                Assert.IsNull(dataModel.Root);
            }
        }
    }
}