using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using DAL.ViewModels;
using TA_Web_2020_API.Helper;

namespace TA_Web_2020_API.Controllers
{
    public class PingController : TABaseAPIController
    {
        [HttpPost]
        [Route("public/api/ping")]
        public IHttpActionResult Ping()
        {
            string TA_Environment = ConfigurationManager.AppSettings["TA-Environment"];
            return new GenerateResponeHelper<string>("Pong: " + TA_Environment, true, Request, HttpStatusCode.OK);
        }
    }
}
