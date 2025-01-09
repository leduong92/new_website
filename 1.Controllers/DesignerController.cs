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

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/Designer")]
    public class DesignerController : TABaseAPIController
    {
        private readonly IDesignerRepository _designerRepository;
        public DesignerController(IDesignerRepository designerRepository) : base()
        {
            _designerRepository = designerRepository;
        }

        [HttpGet]
        public IHttpActionResult GetByURLCode(string URLCode)
        {
            TA.Data2021.Models.Designer item = _designerRepository.GetByURLCode(URLCode);            
            return new GenerateResponeHelper<TA.Data2021.Models.Designer>(item, true, Request, HttpStatusCode.OK);
        }       
        [HttpGet]
        public IHttpActionResult GetByIsActiveToList(bool isActive)
        {           
            List<TA.Data2021.Models.Designer> list = _designerRepository.GetByIsActiveToList(isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Designer>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
