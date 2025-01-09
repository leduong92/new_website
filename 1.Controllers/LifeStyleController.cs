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
    [RoutePrefix("api/LifeStyle")]
    public class LifeStyleController : TABaseAPIController
    {
        private readonly ILifeStyleRepository _lifeStyleRepository;
        public LifeStyleController(ILifeStyleRepository lifeStyleRepository) : base()
        {
            _lifeStyleRepository = lifeStyleRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByURLCode(string URLCode)
        {
            TA.Data2021.Models.LifeStyle item = _lifeStyleRepository.GetByURLCode(URLCode);           
            return new GenerateResponeHelper<TA.Data2021.Models.LifeStyle>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByIsActiveAndRegionToList(bool isActive, string region)
        {          
            List<TA.Data2021.Models.LifeStyle> list = _lifeStyleRepository.GetByIsActiveAndIsActiveTAUSToList(isActive, AppGlobal.InitializationIsActiveTAUS(region));
            return new GenerateResponeHelper<List<TA.Data2021.Models.LifeStyle>>(list, true, Request, HttpStatusCode.OK);
        }
        //[HttpGet]
        //public IHttpActionResult GetByIsActiveToList(bool isActive)
        //{           
        //    List<TA.Data2021.Models.LifeStyle> list = _lifeStyleRepository.GetByIsActiveToList(isActive);
        //    return new GenerateResponeHelper<List<TA.Data2021.Models.LifeStyle>>(list, true, Request, HttpStatusCode.OK);
        //}
        [HttpGet]
        public async Task<IHttpActionResult> GetByIsActiveToList(bool isActive)
        {
            List<TA.Data2021.Models.LifeStyle> list = await _lifeStyleRepository.GetByIsActiveToList(isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.LifeStyle>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
