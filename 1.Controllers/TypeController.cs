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
    [RoutePrefix("api/Type")]
    public class TypeController : TABaseAPIController
    {
        private readonly ITypeRepository _typeRepository;
        public TypeController(ITypeRepository typeRepository) : base()
        {
            _typeRepository = typeRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByID(string ID)
        {
            TA.Data2021.Models.Type item = _typeRepository.GetByID(ID);
            return new GenerateResponeHelper<TA.Data2021.Models.Type>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public TA.Data2021.Models.Type GetByID001(string ID)
        {
            TA.Data2021.Models.Type item = _typeRepository.GetByID(ID);
            return item;
        }

        [HttpGet]
        public IHttpActionResult GetByURLCode(string URLCode)
        {
            TA.Data2021.Models.Type item = _typeRepository.GetByURLCode(URLCode);
            return new GenerateResponeHelper<TA.Data2021.Models.Type>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByIsActiveAndRegionToList(bool isActive, string region)
        {
            List<TA.Data2021.Models.Type> list = _typeRepository.GetByIsActiveAndIsActiveTAUSToList(isActive, AppGlobal.InitializationIsActiveTAUS(region));
            return new GenerateResponeHelper<List<TA.Data2021.Models.Type>>(list, true, Request, HttpStatusCode.OK);
        }
        //[HttpGet]
        //public IHttpActionResult GetByIsActiveToList(bool isActive)
        //{
        //    List<TA.Data2021.Models.Type> list = _typeRepository.GetByIsActiveToList(isActive);
        //    return new GenerateResponeHelper<List<TA.Data2021.Models.Type>>(list, true, Request, HttpStatusCode.OK);
        //}
        [HttpGet]
        public async Task<IHttpActionResult> GetByIsActiveToList(bool isActive)
        {
            List<TA.Data2021.Models.Type> list = await _typeRepository.GetByIsActiveToList(isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Type>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByRegionToList(string region)
        {
            List<TA.Data2021.Models.Type> list = new List<TA.Data2021.Models.Type>();
            bool isActiveUS = AppGlobal.InitializationIsActiveTAUS(region);
            if (isActiveUS == true)
            {
                list = _typeRepository.GetByIsActiveUSToList(isActiveUS);
            }
            else
            {
                bool isActiveInternational = true;
                list = _typeRepository.GetByIsActiveInternationalToList(isActiveInternational);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.Type>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
