using BL.BusinessService;
using DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TA_Web_2020_API._2.APIService;
using TA_Web_2020_API.Helper;
using TA_Web_2020_API.ViewModel;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/common")]
    public class CommonController : TABaseAPIController
    {
        private readonly QuotationBusinessService _quotationServices;
        private readonly SubcribedEmailBusinessService _subcribedEmailServices;
        private readonly ContactBusinessService _contactServices;

        public CommonController(QuotationBusinessService quotationServices, SubcribedEmailBusinessService subcribedEmailServices, ContactBusinessService contactServices)
        {
            _quotationServices = quotationServices;
            _subcribedEmailServices = subcribedEmailServices;
            _contactServices = contactServices;
        }        
        [HttpGet]
        public async Task<IHttpActionResult> GetSubjects()
        {
            try
            {
                bool isDealer = false;
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserType.ToUpper().Equals(LocalUserType.Dealer.ToUpper()))
                {
                    isDealer = true;
                }
                var result = _contactServices.GetContactSubjects(isDealer);
                return new GenerateResponeHelper<ListContactSubject>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Get subjects failed", false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> QuotationRequest(RequestQuoteObj quotationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var context = HttpContext.Current;
                    var jwtModel = await _helper.GenerateJWTViewModel();
                    var result = await _quotationServices.QuotationRequestProcess(quotationViewModel, context, jwtModel);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Request a quote success", true, Request, HttpStatusCode.OK);
                    }
                    else
                    {
                        return new GenerateResponeHelper<string>("Request a quote failed", false, Request, HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SubcribeEmail(SubcribedEmailResigterObj obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jwtModel = await _helper.GenerateJWTViewModel();
                    var result = await _subcribedEmailServices.RegisterSubcribedEmail(obj, jwtModel);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Email has been subcribed", true, Request, HttpStatusCode.OK);
                    }
                    return new GenerateResponeHelper<string>("Cannot subcribe the email: " + obj.Email, false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> SubcribeEmail2021(string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jwtModel = await _helper.GenerateJWTViewModel();
                    var result = await _subcribedEmailServices.RegisterSubcribedEmail2021(email, jwtModel);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Email has been subcribed", true, Request, HttpStatusCode.OK);
                    }
                    return new GenerateResponeHelper<string>("Cannot subcribe the email: " + email, false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> ContactEmail(ContactRequestObj obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _contactServices.SendContactEmail(obj);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Send email success", true, Request, HttpStatusCode.OK);
                    }
                    return new GenerateResponeHelper<string>("Cannot send email: " + obj.Email, false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> ContactEmail2021(string subject, string email, string phone, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactRequestObj obj = new ContactRequestObj();
                    obj.Subject = subject;
                    obj.Email = email;
                    obj.Phone = phone;
                    obj.Message = message;
                    var result = await _contactServices.SendContactEmail(obj);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Send email success", true, Request, HttpStatusCode.OK);
                    }
                    return new GenerateResponeHelper<string>("Cannot send email: " + obj.Email, false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> RalphLaurenInquiries(RalphLaurenInquiriesModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _contactServices.SendRalphLaurenInquiriesEmail(obj);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Send email success", true, Request, HttpStatusCode.OK);
                    }
                    return new GenerateResponeHelper<string>("Cannot send email: " + obj.Email, false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> ApplyJobVN(ApplyJobVNCandidatetObj obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _contactServices.SendApplyVNCandidateEmail(obj);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Send email success", true, Request, HttpStatusCode.OK);
                    }
                    return new GenerateResponeHelper<string>("Cannot send email: " + obj.Email, false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
