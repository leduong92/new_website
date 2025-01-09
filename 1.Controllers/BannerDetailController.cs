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
    [RoutePrefix("api/BannerDetail")]
    public class BannerDetailController : TABaseAPIController
    {
        private readonly IBannerDetailRepository _bannerDetailRepository;
        public BannerDetailController(IBannerDetailRepository bannerDetailRepository) : base()
        {
            _bannerDetailRepository = bannerDetailRepository;
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetByParentIDAndActiveAndRegionToList(int parentID, bool active, string region)
        {
            List<TA.Data2021.Models.BannerDetail> list = new List<TA.Data2021.Models.BannerDetail>();
            if (AppGlobal.InitializationIsActiveTAUS(region) == true)
            {
                list = await _bannerDetailRepository.GetByParentIDAndActiveAndIsUSShowToList(parentID, active, true);
            }
            else
            {
                list = await _bannerDetailRepository.GetByParentIDAndActiveAndIsInternationalShowToList(parentID, active, true);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.BannerDetail>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetByActiveAndRegionToList(bool active, string region)
        {
            List<TA.Data2021.Models.BannerDetail> list = new List<TA.Data2021.Models.BannerDetail>();
            if (AppGlobal.InitializationIsActiveTAUS(region) == true)
            {
                list = await _bannerDetailRepository.GetByActiveAndIsInternationalShowToList(active, true);
            }
            else
            {
                list = await _bannerDetailRepository.GetByActiveAndIsInternationalShowToList(active, true);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.BannerDetail>>(list, true, Request, HttpStatusCode.OK);
        }
    }
}
