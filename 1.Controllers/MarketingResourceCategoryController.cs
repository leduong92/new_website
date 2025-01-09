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
using TA.Data2021.Models;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/MarketingResourceCategory")]
    public class MarketingResourceCategoryController : TABaseAPIController
    {
        private readonly IMarketingResourceCategoryRepository _marketingResourceCategoryRepository;
        public MarketingResourceCategoryController(IMarketingResourceCategoryRepository marketingResourceCategoryRepository) : base()
        {
            _marketingResourceCategoryRepository = marketingResourceCategoryRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByActiveToList(bool active)
        {
            List<TA.Data2021.Models.MarketingResourceCategory> list = _marketingResourceCategoryRepository.GetByActiveToList(active);
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResourceCategory>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByActiveAndRegionToList(bool active, string region)
        {
            List<TA.Data2021.Models.MarketingResourceCategory> list = new List<MarketingResourceCategory>();
            if (AppGlobal.InitializationIsActiveTAUS(region) == true)
            {
                list = _marketingResourceCategoryRepository.GetByActiveAndIsUSShowToList(active, true);
            }
            else
            {
                list = _marketingResourceCategoryRepository.GetByActiveAndIsInternationalShowToList(active, true);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResourceCategory>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpPut]
        public IHttpActionResult UpdateBySQL()
        {
            MarketingResourceCategory model = new MarketingResourceCategory();
            _marketingResourceCategoryRepository.UpdateBySQL(model);
            return new GenerateResponeHelper<string>("", true, Request, HttpStatusCode.OK);
        }

    }
}
