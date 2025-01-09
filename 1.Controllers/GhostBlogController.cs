using BL.CustomExceptions;
using DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TA_Web_2020_API.Helper;
using BL.DTO;
using BL.BUServices;
using System.Web.Script.Serialization;
using TA.Data2021.Repositories;
using TA.Helpers2021;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/GhostBlog")]

    public class GhostBlogController : TABaseAPIController
    {
        private readonly IGhostBlogRepository _ghostBlogRepository;
        public GhostBlogController(IGhostBlogRepository ghostBlogRepository) : base()
        {
            _ghostBlogRepository = ghostBlogRepository;
        }      
        [HttpGet]
        public IHttpActionResult GetByActiveToList(bool active)
        {
            List<TA.Data2021.Models.GhostBlog> result = new List<TA.Data2021.Models.GhostBlog>();
            WebClient webClient = new WebClient();
            string json = webClient.DownloadString("https://theodorealexander.com/Download/GhostBlog.json");
            if (!string.IsNullOrEmpty(json))
            {
                result = JsonConvert.DeserializeObject<List<TA.Data2021.Models.GhostBlog>>(json);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.GhostBlog>>(result, true, Request, HttpStatusCode.OK);
        }
    }
}
