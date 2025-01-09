using BL.BUServices;
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
using BL.DTO;
using TA_Web_2020_API._2.APIService;
using TA_Web_2020_API._3.TAService.AdminTAService;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : TABaseAPIController
    {
        private readonly IAdminAPIService _adminService;

        public AdminController(IAdminAPIService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IHttpActionResult ping()
        {
            try
            {
                return new GenerateResponeHelper<String>("Pong AdminController" + _adminService.ping(), true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("MetaDataController: GetRooms failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> CheckMissingImageProduct(string sendEmailTo = "", bool isNeedResponse = false)//sendEmailTo separated by ;
        {
            try
            {
                if (isNeedResponse && !String.IsNullOrEmpty(sendEmailTo)) {
                    var emails = sendEmailTo.Split(';');
                    foreach (var email in emails)
                    {
                        var sendMail = BL.Helper.SendMail("Theodore Alexander", "no-reply@theodorealexander.com", "Theodore Alexander Staff", email, "Items are missing image: triggered", "Processing... ETA 3 mins");
                    }
                }
                var ret = await _adminService.CheckMissingImageProduct(sendEmailTo);
                if (isNeedResponse)
                {
                    return new GenerateResponeHelper<CheckMissingImageResult>(ret, true, Request, HttpStatusCode.OK);
                }
                else {
                    return new GenerateResponeHelper<string>("OK", true, Request, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("AdminController: CheckMissingImageProduct failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> CheckMissingImageFabric(string sendEmailTo = "", bool isNeedResponse = false)//sendEmailTo separated by ;
        {
            try
            {
                if (isNeedResponse && !String.IsNullOrEmpty(sendEmailTo))
                {
                    var emails = sendEmailTo.Split(';');
                    foreach (var email in emails)
                    {
                        var sendMail = BL.Helper.SendMail("Theodore Alexander", "no-reply@theodorealexander.com", "Theodore Alexander Staff", email, "Fabrics are missing image: triggered", "Processing... ETA 1 mins");
                    }
                }

                var ret = await _adminService.CheckMissingImageFabric(sendEmailTo);
                if (isNeedResponse)
                {
                    return new GenerateResponeHelper<CheckMissingImageResult>(ret, true, Request, HttpStatusCode.OK);
                }
                else
                {
                    return new GenerateResponeHelper<string>("OK", true, Request, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("AdminController: CheckMissingImageProduct failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult SetItemActiveStatus(string sku, bool? isActiveTAUS = null, bool? isActiveINTL = null)
        {
            try
            {
                bool ret =_adminService.SetItemActiveStatus(sku, isActiveTAUS, isActiveINTL);
                return new GenerateResponeHelper<string>("SetItemActiveStatus", ret, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("AdminController: CheckMissingImageProduct failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
