using BL.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace TA_Web_2020_API.Controllers
{
    public class TestController : TABaseAPIController
    {
        private readonly LocatorBusinessService _locatorServices;
        private readonly QuotationBusinessService _quotationServices;

        public TestController(LocatorBusinessService locatorServices, QuotationBusinessService quotationServices)
        {
            _locatorServices = locatorServices;
            _quotationServices = quotationServices;
        }

        [HttpGet]
        [Route("test/api/testemailsendgrid")]
        public async Task<IHttpActionResult> SendGridEmail(string email, string apikey = null, string password = null, bool isSSL = true)
        {
            var sendMail = await BL.Helper.SendGridMail("Theodore Alexander", "no-reply@theodorealexander.com", "Tan", email, "TEST EMAIL", "Hello send grid", apikey, password, isSSL);
            if (sendMail)
            {
                return Ok("OK");
            }
            else
            {
                return BadRequest("NG");
            }
        }

        [HttpGet]
        [Route("test/api/testemail")]
        public async Task<IHttpActionResult> SendEmail(string emailTo, string subject = "", string message = "")
        {
            var sendMail = await BL.Helper.SendMail("Theodore Alexander", "no-reply@theodorealexander.com", "Theodore Alexander Staff", emailTo, subject ?? "TEST EMAIL", message ?? "Hello");
            if (sendMail)
            {
                return Ok("OK");
            }
            else
            {
                return BadRequest("NG");
            }
        }

        [HttpGet]
        [Route("test/api/testmaindatabase")]
        public IHttpActionResult TestMainDatabase()
        {
            try
            {
                var ret = _locatorServices.GetSelectBoxCountries().FirstOrDefault(); ;
                if (ret != null)
                {
                    return Ok("OK");
                }
                else
                {
                    return BadRequest("NG");
                }
            }
            catch (Exception ex) {
                return BadRequest("NG");
            }
        }

        [HttpGet]
        [Route("test/api/testquotationdatabase")]
        public IHttpActionResult TestQuotationDatabase()
        {
            try
            {
                var ret = _quotationServices.GetQuotations().FirstOrDefault();
                if (ret != null)
                {
                    return Ok("OK");
                }
                else
                {
                    return BadRequest("NG");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("NG");
            }

        }
    }
}
