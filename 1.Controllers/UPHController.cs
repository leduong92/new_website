using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BL.BusinessService;
using BL.DTO;
using DAL.ViewModels;
using TA_Web_2020_API.Helper;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/uph")]
    public class UPHController : TABaseAPIController
    {
        private readonly FabricBusinessService _fabricServices;

        public UPHController(FabricBusinessService fabricServices)
        {
            _fabricServices = fabricServices;
        }
        [HttpPost]
        public IHttpActionResult GetFabrics(RequestFabricObj requestFabricObj)
        {
            try
            {
                var fabrics = _fabricServices.PaginationFabrics(requestFabricObj);
                return new GenerateResponeHelper<QueryFabricLeatherTrimViewModel>(fabrics, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Not found: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetFabrics2021()
        {
            RequestFabricObj requestFabricObj = new RequestFabricObj();
            requestFabricObj.Ascending = true;
            requestFabricObj.Category = "FAB";
            requestFabricObj.Colors = "";
            requestFabricObj.Contents = "";
            requestFabricObj.InStock = false;
            requestFabricObj.KeyWord = "";
            requestFabricObj.OrderBy = "Fabric";
            requestFabricObj.PFP = true;
            requestFabricObj.PageNum = 1;
            requestFabricObj.PageSize = 30;
            requestFabricObj.Patterns = "";
            try
            {
                var fabrics = _fabricServices.PaginationFabrics(requestFabricObj);
                return new GenerateResponeHelper<QueryFabricLeatherTrimViewModel>(fabrics, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Not found: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUpholsteryFilter()
        {
            try
            {
                var fabricColours = await _fabricServices.GetFabricColour();
                var fabricPattern = await _fabricServices.GetFabricPattern();
                var fabricContent = await _fabricServices.GetFabricContent();

                var returnedModel = new UpholsteryFilter
                {
                    FabricColours = fabricColours,
                    FabricPatterns = fabricPattern,
                    FabricContents = fabricContent
                };
                return new GenerateResponeHelper<UpholsteryFilter>(returnedModel, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Not found", false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetFabric(string code)
        {
            try
            {
                UPHFabricDTO returnedModel = _fabricServices.GetFabric(code);
                return new GenerateResponeHelper<UPHFabricDTO>(returnedModel, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Not found", false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
