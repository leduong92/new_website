using BL.CustomExceptions;
using DAL.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TA_Web_2020_API.Helper;
using BL.DTO;
using BL.BUServices;
using System.Web.Script.Serialization;
using TA.Data2021.Repositories;
using System.Collections.Generic;
using TA.Helpers2021;
using System.IO;
using Newtonsoft.Json;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/Shape")]
    public class ShapeController : TABaseAPIController
    {
        private readonly IShapeRepository _shapeRepository;
        public ShapeController(IShapeRepository shapeRepository) : base()
        {
            _shapeRepository = shapeRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByURLCode(string URLCode)
        {
            TA.Data2021.Models.Shape item = _shapeRepository.GetByURLCode(URLCode);            
            return new GenerateResponeHelper<TA.Data2021.Models.Shape>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByIsActiveAndRegionToList(bool isActive, string region)
        {           
            List<TA.Data2021.Models.Shape> list = _shapeRepository.GetByIsActiveAndIsActiveTAUSToList(isActive, AppGlobal.InitializationIsActiveTAUS(region));
            return new GenerateResponeHelper<List<TA.Data2021.Models.Shape>>(list, true, Request, HttpStatusCode.OK);
        }

        //[HttpGet]
        //public IHttpActionResult GetByIsActiveToList(bool isActive)
        //{          
        //    List<TA.Data2021.Models.Shape> list = _shapeRepository.GetByIsActiveToList(isActive);
        //    return new GenerateResponeHelper<List<TA.Data2021.Models.Shape>>(list, true, Request, HttpStatusCode.OK);
        //}
        [HttpGet]
        public async Task<IHttpActionResult> GetByIsActiveToList(bool isActive)
        {
            List<TA.Data2021.Models.Shape> list = await _shapeRepository.GetByIsActiveToList(isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Shape>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
