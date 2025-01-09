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
    [RoutePrefix("api/FurnitureCare")]
    public class FurnitureCareController : TABaseAPIController
    {
        private readonly IFurnitureCareRepository _furnitureCareRepository;
        public FurnitureCareController(IFurnitureCareRepository furnitureCareRepository) : base()
        {
            _furnitureCareRepository = furnitureCareRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByActiveAndIsItemToList(bool active, bool isItem)
        {
            List<TA.Data2021.Models.FurnitureCare> result = new List<TA.Data2021.Models.FurnitureCare>();
            result = _furnitureCareRepository.GetByActiveAndIsItemToList(active, isItem);
            return new GenerateResponeHelper<List<TA.Data2021.Models.FurnitureCare>>(result, true, Request, HttpStatusCode.OK);
        }
    }
}
