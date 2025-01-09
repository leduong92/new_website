using BL.BUServices;
using BL.DTO;
using DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TA_Web_2020_API.Helper;
using BL.Dto;
using TA_Web_2020_API._2.APIService;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/metadata")]
    public class MetadataController : TABaseAPIController
    {
        private readonly IMetadataAPIService _metaDataService;

        public MetadataController(IMetadataAPIService metaDataService)
        {
            _metaDataService = metaDataService;
        }

        [HttpGet]
        public IHttpActionResult GetRooms()
        {
            try
            {
                var result = _metaDataService.GetAllRooms();
                return new GenerateResponeHelper<List<RoomAndUsageDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("MetaDataController: GetRooms failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetTypes()
        {
            try
            {
                var result = _metaDataService.GetAllTypes();
                return new GenerateResponeHelper<List<TypeDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("MetaDataController: GetTypes failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetStyles()
        {
            try
            {
                var result = _metaDataService.GetAllStyles();
                return new GenerateResponeHelper<List<StyleDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("MetaDataController: GetStyles failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetLifeStyles()
        {
            try
            {
                var result = _metaDataService.GetAllLifeStyles();
                return new GenerateResponeHelper<List<LifeStyleDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("MetaDataController: GetLifeStyles failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetCollections()
        {
            try
            {
                var result = _metaDataService.GetAllCollections();
                return new GenerateResponeHelper<List<CollectionDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("MetaDataController: GetCollections failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetBrands()
        {
            try
            {
                var result = _metaDataService.GetAllBrands();
                return new GenerateResponeHelper<List<BrandDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("MetaDataController: GetBrands failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetDesigners()
        {
            try
            {
                var result = _metaDataService.GetAllDesigners();
                return new GenerateResponeHelper<List<DesignerDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("MetaDataController: GetDesigners failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetOptionGroups()
        {
            try
            {
                var result = _metaDataService.GetAllOptionGroups();
                return new GenerateResponeHelper<List<OptionGroupDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetOptionGroups: GetDesigners failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public IHttpActionResult GetAllItemResourceFiles()
        {
            try
            {
                var result = _metaDataService.GetAllItemResourceFiles();
                return new GenerateResponeHelper<List<ItemResourceFileDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetOptionGroups: GetDesigners failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetItemResourceFileByID(Guid id)
        {
            try
            {
                var result = _metaDataService.GetItemResourceFileByID(id);
                return new GenerateResponeHelper<ItemResourceFileDto>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetOptionGroups: GetDesigners failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetRulesByItemResourceFileID(Guid ItemResourceFileID)
        {
            try
            {
                var result = _metaDataService.GetRulesByItemResourceFileID(ItemResourceFileID);
                return new GenerateResponeHelper<List<ItemResourceFile_RuleDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetOptionGroups: GetDesigners failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateItemResourceFileRule(ItemResourceFile_RuleDto ruleDTO)
        {
            try
            {
                var result = _metaDataService.UpdateItemResourceFileRule(ruleDTO);
                return new GenerateResponeHelper<bool>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("UpdateItemResourceFileRule failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public IHttpActionResult AddItemResourceFileRule(ItemResourceFile_RuleDto ruleDTO)
        {
            try
            {
                var result = _metaDataService.AddItemResourceFileRule(ruleDTO);
                return new GenerateResponeHelper<Guid?>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("AddItemResourceFileRule failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult DeleteItemResourceFileRule(Guid id)
        {
            try
            {
                var result = _metaDataService.DeleteItemResourceFileRule(id);
                return new GenerateResponeHelper<bool>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("DeleteItemResourceFileRule failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public IHttpActionResult UpdateItemResourceFile(ItemResourceFileDto dto)
        {
            try
            {
                var result = _metaDataService.UpdateItemResourceFile(dto);
                return new GenerateResponeHelper<bool>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("UpdateItemResourceFile failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public IHttpActionResult AddItemResourceFile(ItemResourceFileDto dto)
        {
            try
            {
                var result = _metaDataService.AddItemResourceFile(dto);
                return new GenerateResponeHelper<Guid?>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("AddItemResourceFile failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult DeleteItemResourceFile(Guid id)
        {
            try
            {
                var result = _metaDataService.DeleteItemResourceFile(id);
                return new GenerateResponeHelper<bool>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("DeleteItemResourceFile failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult FilterItemsByRule(Guid id)
        {
            try
            {
                var result = _metaDataService.FilterItemsByRule(id);
                return new GenerateResponeHelper<List<ItemDto>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("FilterItemsByRule failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
