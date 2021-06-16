using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SearchYourTrack.Helpers;
using SearchYourTrack.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SearchYourTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            SongSearchModel songsSearchModel = new SongSearchModel();
            //Following line will reset the UI as no search performed
            songsSearchModel.RecordCount = -1;
            return View(songsSearchModel);
        }

        [HttpPost]
        public IActionResult FetchSongData(SongSearchModel songsSearchModel)
        {
            if (!ModelState.IsValid || songsSearchModel.SearchInput.Length>50)
            {
                ViewData["ErrorMessage"] = "Invalid or no input received.";
            }
            else
            {
                var endPoint = "";
                var inputPramString = "";
                //Prepare url based on search criteria and inputs
                switch (songsSearchModel.SearchByOption)
                {
                    case ("Pattern"):
                        endPoint = "songs.xml?pattern=";
                        inputPramString = songsSearchModel.SearchInput;
                        break;
                    case ("Artists"):
                        endPoint = "songs/byartists.xml?artists=";
                        inputPramString = Regex.Replace(songsSearchModel.SearchInput, @"\s+", ",");
                        break;
                    default:
                        endPoint = "songs.xml?pattern=";
                        inputPramString = songsSearchModel.SearchInput;
                        break;
                }

                Root root = new Root();
                endPoint = endPoint + inputPramString.ToLower();
                ApiRequestHelper request = new ApiRequestHelper(_logger, _configuration);
                //Hit api to get data from
                var response = request.GetREST(endPoint);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(response.Content);
                    string jsonText = JsonConvert.SerializeXmlNode(doc.ChildNodes[1], Newtonsoft.Json.Formatting.None, false);
                    root = JsonConvert.DeserializeObject<Root>(jsonText);
                }
                songsSearchModel.Root = root;

            }
            return View("Index", songsSearchModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
