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
    [RoutePrefix("api/Collection")]
    public class CollectionController : TABaseAPIController
    {
        private readonly ICollectionRepository _collectionRepository;
        public CollectionController(ICollectionRepository collectionRepository) : base()
        {
            _collectionRepository = collectionRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByID(string ID)
        {
            TA.Data2021.Models.Collection item = _collectionRepository.GetByID(ID);
            return new GenerateResponeHelper<TA.Data2021.Models.Collection>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByID()
        {
            string ID = "";
            TA.Data2021.Models.Collection item = _collectionRepository.GetByID(ID);
            return new GenerateResponeHelper<TA.Data2021.Models.Collection>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByURLCode(string URLCode)
        {
            TA.Data2021.Models.Collection item = _collectionRepository.GetByURLCode(URLCode);
            return new GenerateResponeHelper<TA.Data2021.Models.Collection>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByIsActiveAndRegionToList(bool isActive, string region)
        {
            List<TA.Data2021.Models.Collection> list = _collectionRepository.GetByIsActiveAndIsActiveTAUSToList(isActive, AppGlobal.InitializationIsActiveTAUS(region));
            return new GenerateResponeHelper<List<TA.Data2021.Models.Collection>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetByIsActiveToList(bool isActive)
        {
            List<TA.Data2021.Models.Collection> list = await _collectionRepository.GetByIsActiveToList(isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Collection>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetByBrand_IDAndIsActiveToList(string brand_ID, bool isActive)
        {
            List<TA.Data2021.Models.Collection> list = await _collectionRepository.GetByBrand_IDAndIsActiveToList(brand_ID, isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Collection>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByBrand_IDAndIsActiveAndByRegionToList(string brand_ID, bool isActive, bool isActiveTAUS)
        {
            List<TA.Data2021.Models.Collection> list = _collectionRepository.GetByBrand_IDAndIsActiveAndByRegionToList(brand_ID, isActive, isActiveTAUS);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Collection>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByBrand_IDAndIsActiveAndIsActiveTAUSToList(string brand_ID, bool isActive, string region)
        {
            List<TA.Data2021.Models.Collection> list = _collectionRepository.GetByBrand_IDAndIsActiveAndIsActiveTAUSToList(brand_ID, isActive, AppGlobal.InitializationIsActiveTAUS(region));
            return new GenerateResponeHelper<List<TA.Data2021.Models.Collection>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByBrand_IDAndIsActiveAndIsActiveTAUSAndItemCountToList(string brand_ID, bool isActive, string region)
        {
            List<TA.Data2021.Models.Collection> list = _collectionRepository.GetByBrand_IDAndIsActiveAndIsActiveTAUSAndItemCountToList(brand_ID, isActive, AppGlobal.InitializationIsActiveTAUS(region), AppGlobal.ItemCount);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Collection>>(list, true, Request, HttpStatusCode.OK);
        }

        [HttpGet]
        public IHttpActionResult GetByIsActiveAndIsActiveTAUSCovertMenuToList(bool isActive, bool isActiveTAUS)
        {
            List<TA.Data2021.Models.Collection> list = _collectionRepository.GetByIsActiveAndIsActiveTAUSCovertMenuToList(isActive, isActiveTAUS);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Collection>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
