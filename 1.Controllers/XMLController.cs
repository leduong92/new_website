using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using TA_Web_2020_API.Helper;
using TA_Web_2020_API.Models;
using DAL.ViewModels;
using BL.BusinessService;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/XML")]
    public class XMLController : TABaseAPIController
    {
        private readonly GeneralBusinessService _generalServices;

        public XMLController(GeneralBusinessService generalServices)
        {
            _generalServices = generalServices;
        }
        [HttpPost]
        public async Task<IHttpActionResult> GetXml(MenuRequestObj model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new GenerateResponeHelper<string>("File name must be not null", false, Request, HttpStatusCode.BadRequest);
                }

                //check file exists 
                if (! await BL.Helper.CheckFileExistsAsync(model.XmlPath))
                {
                    return new GenerateResponeHelper<string>("Invaild file", false, Request, HttpStatusCode.BadRequest);
                }
                //Read and parse to json
                var jsonObj = _generalServices.ConvertJsonStringToJson(model.XmlPath);
                return new GenerateResponeHelper<JObject>(jsonObj, true, Request, HttpStatusCode.OK);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
