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
    [RoutePrefix("api/MarketingResource")]
    public class MarketingResourceController : TABaseAPIController
    {
        private readonly IMarketingResourceRepository _marketingResourceRepository;
        public MarketingResourceController(IMarketingResourceRepository marketingResourceRepository) : base()
        {
            _marketingResourceRepository = marketingResourceRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByParentIDAndActiveToList(int parentID, bool active)
        {
            List<TA.Data2021.Models.MarketingResource> list = _marketingResourceRepository.GetByParentIDAndActiveToList(parentID, active);
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResource>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByActiveToList(bool active)
        {
            List<TA.Data2021.Models.MarketingResource> list = _marketingResourceRepository.GetByActiveToList(active);
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResource>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByActiveAndRegionToList(bool active, string region)
        {
            List<TA.Data2021.Models.MarketingResource> list = new List<TA.Data2021.Models.MarketingResource>();
            if (AppGlobal.InitializationIsActiveTAUS(region) == true)
            {
                list = _marketingResourceRepository.GetByActiveAndIsUSShowToList(active, true);
            }
            else
            {
                list = _marketingResourceRepository.GetByActiveAndIsInternationalShowToList(active, true);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResource>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
