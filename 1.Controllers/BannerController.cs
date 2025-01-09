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
    [RoutePrefix("api/Banner")]
    public class BannerController : TABaseAPIController
    {
        private readonly IBannerRepository _bannerRepository;
        public BannerController(IBannerRepository bannerRepository) : base()
        {
            _bannerRepository = bannerRepository;
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetByID(int ID)
        {
            TA.Data2021.Models.Banner result = new TA.Data2021.Models.Banner();
            result = await _bannerRepository.GetByID(ID);
            return new GenerateResponeHelper<TA.Data2021.Models.Banner>(result, true, Request, HttpStatusCode.OK);
        }
    }
}
