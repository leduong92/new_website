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
    [RoutePrefix("api/Item_Fabric")]
    public class Item_FabricController : TABaseAPIController
    {
        private readonly IItem_FabricRepository _item_FabricRepository;
        public Item_FabricController(IItem_FabricRepository item_FabricRepository) : base()
        {
            _item_FabricRepository = item_FabricRepository;
        }

        [HttpGet]
        public IHttpActionResult GetByItemIDAndIsActiveToList(string ItemID, bool isActive)
        {

            List<TA.Data2021.Models.Item_Fabric> list = _item_FabricRepository.GetByItemIDAndIsActiveToList(ItemID, isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.Item_Fabric>>(list, true, Request, HttpStatusCode.OK);
        }

    }
}
