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
    [RoutePrefix("api/MarketingResourceDetail")]
    public class MarketingResourceDetailController : TABaseAPIController
    {
        private readonly IMarketingResourceDetailRepository _marketingResourceDetailRepository;
        public MarketingResourceDetailController(IMarketingResourceDetailRepository marketingResourceDetailRepository) : base()
        {
            _marketingResourceDetailRepository = marketingResourceDetailRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByParentIDAndActiveToList(int parentID, bool active)
        {
            List<TA.Data2021.Models.MarketingResourceDetail> list = _marketingResourceDetailRepository.GetByParentIDAndActiveToList(parentID, active);
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResourceDetail>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByActiveToList(bool active)
        {
            List<TA.Data2021.Models.MarketingResourceDetail> list = _marketingResourceDetailRepository.GetByActiveToList(active);
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResourceDetail>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByActiveAndRegionToList(bool active, string region)
        {
            List<TA.Data2021.Models.MarketingResourceDetail> list = new List<TA.Data2021.Models.MarketingResourceDetail>();
            if (AppGlobal.InitializationIsActiveTAUS(region) == true)
            {
                list = _marketingResourceDetailRepository.GetByActiveAndIsUSShowToList(active, true);
            }
            else
            {
                list = _marketingResourceDetailRepository.GetByActiveAndIsInternationalShowToList(active, true);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResourceDetail>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByParentIDAndActiveAndRegionToList(int parentID, bool active, string region)
        {
            List<TA.Data2021.Models.MarketingResourceDetail> list = new List<TA.Data2021.Models.MarketingResourceDetail>();
            if (AppGlobal.InitializationIsActiveTAUS(region) == true)
            {
                list = _marketingResourceDetailRepository.GetByParentIDAndActiveAndIsUSShowToList(parentID, active, true);
            }
            else
            {
                list = _marketingResourceDetailRepository.GetByParentIDAndActiveAndIsInternationalShowToList(parentID, active, true);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.MarketingResourceDetail>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
