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
    [RoutePrefix("api/Style")]
    public class StyleController : TABaseAPIController
    {
        private readonly IStyleRepository _styleRepository;
        public StyleController(IStyleRepository styleRepository) : base()
        {
            _styleRepository = styleRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByURLCode(string URLCode)
        {
            TA.Data2021.Models.Style item = _styleRepository.GetByURLCode(URLCode);           
            return new GenerateResponeHelper<TA.Data2021.Models.Style>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByIsActiveAndRegionToList(bool isActive, string region)
        {           
            List<TA.Data2021.Models.Style> list = _styleRepository.GetByIsActiveAndIsActiveTAUSToList(isActive, AppGlobal.InitializationIsActiveTAUS(region));
            return new GenerateResponeHelper<List<TA.Data2021.Models.Style>>(list, true, Request, HttpStatusCode.OK);
        }
        //[HttpGet]
        //public IHttpActionResult GetByIsActiveToList(bool isActive)
        //{            
        //    List<TA.Data2021.Models.Style> list = _styleRepository.GetByIsActiveToList(isActive);
        //    return new GenerateResponeHelper<List<TA.Data2021.Models.Style>>(list, true, Request, HttpStatusCode.OK);
        //}
        [HttpGet]
        public async Task<IHttpActionResult> GetByIsActiveToList(bool isActive)
        {
            List<TA.Data2021.Models.Style> list = await _styleRepository.GetByIsActiveToList(isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Style>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
