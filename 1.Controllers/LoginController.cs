using BL.BUServices;
using BL.BusinessService;
using DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using TA_Web_2020_API.Helper;

namespace TA_Web_2020_API.Controllers
{
    //[RoutePrefix("public/api/login")]
    public class LoginController : TABaseAPIController
    {
        private readonly UserBusinessService _userServices;
        private readonly ContactBusinessService _contactBusinessServices;
        private readonly IWishlistlItemService _wishListBusinessService;

        public LoginController(UserBusinessService userServices, ContactBusinessService contactServices, IWishlistlItemService wishListBusinessService)
        {
            _userServices = userServices;
            _contactBusinessServices = contactServices;
            _wishListBusinessService = wishListBusinessService;
        }
        private string GetClientIp(HttpRequestMessage request = null)
        {
            try
            {
                if (request == null)
                {
                    return null;
                }

                if (request.Properties.ContainsKey("MS_HttpContext"))
                {
                    return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                }
                else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
                {
                    RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                    return prop.Address;
                }
                else if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) {
                BL.Helper.SendErrorEmail(Request, ex);
                return null;
            }
        }

        [HttpPost]
        [Route("public/api/login")]
        public async Task<IHttpActionResult> Authenticate(LoginModel model)
        {
            try
            {
                JsonWebTokenModule jwtModule = new JsonWebTokenModule();
                model.Password = BL.Helper.EncryptPassword(model.Password);
                var checkIpAddressExists = Request.Headers.TryGetValues("Ip_Address", out IEnumerable<string> lsHeaders);
                string IpAddress;
                if (checkIpAddressExists)
                {
                    IpAddress = lsHeaders.FirstOrDefault();
                }
                else
                {
                    IpAddress = GetClientIp(Request);
                }
                if (String.IsNullOrEmpty(IpAddress))
                {
                    IpAddress = ConfigurationManager.AppSettings["DefaultIp"]; //Default Ip Address
                }
                var user = await _userServices.Login(model, IpAddress);
                if (user != null)
                {
                    //push the wishlist
                    //Create new wishlist if needed
                    if (model.WishListItems != null && model.WishListItems.Count > 0)
                    {
                        CreateWishListModel createWishListModel = new CreateWishListModel { WishListName = "WishList" };
                        Guid wishListId = await _wishListBusinessService.AddWishList(user.ID, createWishListModel);
                        var addItemsToWishList = await _wishListBusinessService.AddItemListToWishList(wishListId, model.WishListItems);
                    }

                    if (!user.EmailVerified)
                    {
                        return new GenerateResponeHelper<string>("Account is not activated. Please check registered email to active.", false, Request, HttpStatusCode.BadRequest);
                    }
                    if (!user.AccountEnabled)
                    {
                        return new GenerateResponeHelper<string>("Account is disabled", false, Request, HttpStatusCode.BadRequest);
                    }                   
                    string token = await jwtModule.GenerateToken(user.ID, Request);
                    var currentClaim = jwtModule.GetPrincipal(token);
                    var identity = jwtModule.GenerateIdentity(currentClaim);
                    SetPrincipal(identity);

                    JWTIdentityViewModel jwtUser = await _helper.GenerateJWTViewModel();
                    jwtUser.AccountNumber = user.AccountNumber;
                    jwtUser.Region= model.Region;
                    return new GenerateResponeHelper<dynamic>(new { Token = token, UserCredential = jwtUser }, true, Request, HttpStatusCode.OK);
                }
                return new GenerateResponeHelper<string>("Invalid username/email/password", false, Request, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(model));
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("public/api/login2022")]
        public async Task<IHttpActionResult> Authenticate2022()
        {
            LoginModel model = new LoginModel();
            model.UserName = "solo.vera.kzn@gmail.com";
            model.Password = "Solo2022";
            try
            {                
                JsonWebTokenModule jwtModule = new JsonWebTokenModule();
                model.Password = BL.Helper.EncryptPassword(model.Password);
                var checkIpAddressExists = Request.Headers.TryGetValues("Ip_Address", out IEnumerable<string> lsHeaders);
                string IpAddress;
                if (checkIpAddressExists)
                {
                    IpAddress = lsHeaders.FirstOrDefault();
                }
                else
                {
                    IpAddress = GetClientIp(Request);
                }
                if (String.IsNullOrEmpty(IpAddress))
                {
                    IpAddress = ConfigurationManager.AppSettings["DefaultIp"]; //Default Ip Address
                }
                var user = await _userServices.Login(model, IpAddress);
                if (user != null)
                {
                    //push the wishlist
                    //Create new wishlist if needed
                    if (model.WishListItems != null && model.WishListItems.Count > 0)
                    {
                        CreateWishListModel createWishListModel = new CreateWishListModel { WishListName = "WishList" };
                        Guid wishListId = await _wishListBusinessService.AddWishList(user.ID, createWishListModel);
                        var addItemsToWishList = await _wishListBusinessService.AddItemListToWishList(wishListId, model.WishListItems);
                    }

                    if (!user.EmailVerified)
                    {
                        return new GenerateResponeHelper<string>("Account is not activated. Please check registered email to active.", false, Request, HttpStatusCode.BadRequest);
                    }
                    if (!user.AccountEnabled)
                    {
                        return new GenerateResponeHelper<string>("Account is disabled", false, Request, HttpStatusCode.BadRequest);
                    }
                    string token = await jwtModule.GenerateToken(user.ID, Request);
                    var currentClaim = jwtModule.GetPrincipal(token);
                    var identity = jwtModule.GenerateIdentity(currentClaim);
                    SetPrincipal(identity);

                    JWTIdentityViewModel jwtUser = await _helper.GenerateJWTViewModel();
                    jwtUser.AccountNumber = user.AccountNumber;
                    return new GenerateResponeHelper<dynamic>(new { Token = token, UserCredential = jwtUser }, true, Request, HttpStatusCode.OK);
                }
                return new GenerateResponeHelper<string>("Invalid username/email/password", false, Request, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(model));
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }


        private void SetPrincipal(JWTAuthenIdentity identity)
        {
            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;
            if (Thread.CurrentPrincipal.Identity is JWTAuthenIdentity authenticationIdentity)
            {
                authenticationIdentity.CountryId = identity.CountryId;
                authenticationIdentity.UserId = identity.UserId;
                authenticationIdentity.UserName = identity.UserName;
                authenticationIdentity.AccountNumber = identity.AccountNumber;
                authenticationIdentity.Email = identity.Email;
                authenticationIdentity.FirstName = identity.FirstName;
                authenticationIdentity.LastName = identity.LastName;
                authenticationIdentity.UserType = identity.UserType;
                authenticationIdentity.Region = identity.Region;
                authenticationIdentity.IsShowInStock = identity.IsShowInStock;
                authenticationIdentity.Country = identity.Country;
                authenticationIdentity.CountryFullName = identity.CountryFullName;
                authenticationIdentity.ContinentId = identity.ContinentId;
                authenticationIdentity.ExclusivityGroupId = identity.ExclusivityGroupId;
                authenticationIdentity.RequestIpAddress = identity.RequestIpAddress;
                authenticationIdentity.ResponseIpAddress = identity.ResponseIpAddress;
            }
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = genericPrincipal;
            }
        }

        [HttpGet]
        [Route("api/loginByToken/")]
        public async Task<IHttpActionResult> AuthenticateByToken()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                return new GenerateResponeHelper<JWTIdentityViewModel>(jwtModel, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/LoginByUserId/")]
        public async Task<IHttpActionResult> LoginByUserId(string userId)
        {
            return await this.ImpersonateUser(userId, true);
        }

        [HttpGet]
        [Route("api/impersonate/{Id}")]
        public async Task<IHttpActionResult> ImpersonateUser(string Id, bool isByPass = false)
        {
            try {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if ((jwtModel != null && jwtModel.UserName == "admin") || isByPass)
                {
                    if (Id == null || Id == "undefined") 
                    {
                        return new GenerateResponeHelper<string>("Impersonate failed", true, Request, HttpStatusCode.BadRequest);
                    }
                    Guid? userId = BL.Helper.TryParseStringToGuid(Id);
                    JsonWebTokenModule jwtModule = new JsonWebTokenModule();
                    string token = await jwtModule.GenerateToken(userId.Value, Request);
                    var currentClaim = jwtModule.GetPrincipal(token);
                    var identity = jwtModule.GenerateIdentity(currentClaim);
                    SetPrincipal(identity);
                    JWTIdentityViewModel jwtUser = await _helper.GenerateJWTViewModel();
                    return new GenerateResponeHelper<dynamic>(new { Token = token, UserCredential = jwtUser }, true, Request, HttpStatusCode.OK);
                }
                else
                {
                    return new GenerateResponeHelper<string>("Impersonate failed", true, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("public/api/verifyemail/{Id}")]
        public async Task<IHttpActionResult> VerifyRegisterEmail(string Id)
        {
            try {
                Guid? userId = BL.Helper.TryParseStringToGuid(Id);
                if (userId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invalid user", false, Request, HttpStatusCode.NotFound);
                }
                JsonWebTokenModule jwtModule = new JsonWebTokenModule();
                var checkIpAddressExists = Request.Headers.TryGetValues("Ip_Address", out IEnumerable<string> lsHeaders);
                string IpAddress;
                if (checkIpAddressExists)
                {
                    IpAddress = lsHeaders.FirstOrDefault();
                }
                else
                {
                    IpAddress = GetClientIp(Request);
                }
                if (String.IsNullOrEmpty(IpAddress))
                {
                    IpAddress = ConfigurationManager.AppSettings["DefaultIp"]; //Default Ip Address
                }
                var user = await _userServices.VerifyRegisterEmail((Guid)userId, IpAddress);
                if (user == null)
                {
                    return new GenerateResponeHelper<string>("Invalid user", false, Request, HttpStatusCode.NotFound);
                }

                string token = await jwtModule.GenerateToken(user.ID, Request);
                var currentClaim = jwtModule.GetPrincipal(token);
                var identity = jwtModule.GenerateIdentity(currentClaim);
                SetPrincipal(identity);

                JWTIdentityViewModel jwtUser = await _helper.GenerateJWTViewModel();

                return new GenerateResponeHelper<dynamic>(new { Token = token, UserCredential = jwtUser }, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("public/api/RegisterUserAndLoginByProvider")]
        public async Task<IHttpActionResult> RegisterUserByProvider(RegisterUserObj registerUserObj)
        {
            try
            {
                string validateMessage = await _userServices.ValidateRegisterUser(registerUserObj);
                if (!String.IsNullOrEmpty(validateMessage))
                {
                    return new GenerateResponeHelper<string>(validateMessage, false, Request, HttpStatusCode.BadRequest);
                }

                var checkIpAddressExists = Request.Headers.TryGetValues("Ip_Address", out IEnumerable<string> lsHeaders);
                string IpAddress;
                if (checkIpAddressExists)
                {
                    IpAddress = lsHeaders.FirstOrDefault();
                }
                else
                {
                    IpAddress = GetClientIp(Request);
                }
                if (String.IsNullOrEmpty(IpAddress))
                {
                    IpAddress = ConfigurationManager.AppSettings["DefaultIp"]; //Default Ip Address
                }

                Guid userId = await _userServices.RegisterUser(registerUserObj, IpAddress, false);
                if (userId != Guid.Empty)
                {
                    //push the wishlist
                    if (registerUserObj.WishListItems != null && registerUserObj.WishListItems.Count > 0)
                    {
                        CreateWishListModel createWishListModel = new CreateWishListModel { WishListName = "WishList" };
                        Guid wishListId = await _wishListBusinessService.AddWishList(userId, createWishListModel);
                        var addItemsToWishList = await _wishListBusinessService.AddItemListToWishList(wishListId, registerUserObj.WishListItems);
                    }
                    return await this.VerifyRegisterEmail(userId.ToString());
                }
                else
                {
                    return new GenerateResponeHelper<string>(
                        userId != Guid.Empty ? "Register user success" : "RegisterUserAndLoginByProvider failed: cannot create user",
                        userId != Guid.Empty, Request,
                        userId != Guid.Empty ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(registerUserObj));
                return new GenerateResponeHelper<string>("RegisterUserAndLoginByProvider failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("public/api/RegisterUser")]
        public async Task<IHttpActionResult> RegisterUser(RegisterUserObj registerUserObj)
        {
            try
            {
                string validateMessage = await _userServices.ValidateRegisterUser(registerUserObj);
                if (!String.IsNullOrEmpty(validateMessage))
                {
                    return new GenerateResponeHelper<string>(validateMessage, false, Request, HttpStatusCode.BadRequest);
                }

                var checkIpAddressExists = Request.Headers.TryGetValues("Ip_Address", out IEnumerable<string> lsHeaders);
                string IpAddress;
                if (checkIpAddressExists)
                {
                    IpAddress = lsHeaders.FirstOrDefault();
                }
                else
                {
                    IpAddress = GetClientIp(Request);
                }
                if (String.IsNullOrEmpty(IpAddress))
                {
                    IpAddress = ConfigurationManager.AppSettings["DefaultIp"]; //Default Ip Address
                }

                Guid userId = await _userServices.RegisterUser(registerUserObj, IpAddress, true);
                if (userId != Guid.Empty) {
                    //push the wishlist
                    if (registerUserObj.WishListItems != null && registerUserObj.WishListItems.Count > 0)
                    {
                        CreateWishListModel createWishListModel = new CreateWishListModel { WishListName = "WishList" };
                        Guid wishListId = await _wishListBusinessService.AddWishList(userId, createWishListModel);
                        var addItemsToWishList = await _wishListBusinessService.AddItemListToWishList(wishListId, registerUserObj.WishListItems);
                    }
                }
                return new GenerateResponeHelper<string>(
                    userId != Guid.Empty ? "Register user success" : "Register user failed",
                    userId != Guid.Empty, Request,
                    userId != Guid.Empty ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(registerUserObj));
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("public/api/GetUserIdByEmail")]
        public async Task<IHttpActionResult> GetUserIdByEmail(string email)
        {
            try
            {
                var rtn = await _userServices.CheckEmailExists(email);
                return new GenerateResponeHelper<Guid?>(rtn, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Get CheckEmailExists failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("public/api/GetUserTypes")]
        public async Task<IHttpActionResult> GetUserTypes()
        {
            try
            {
                var rtn = await _userServices.GetUserTypesForRegisterUser();
                return new GenerateResponeHelper<UserTypeForRegisterUserViewModel>(rtn, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Get UserType failed", false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("public/api/SendTradeEnquiries")]
        public async Task<IHttpActionResult> SendTradeEnquiries(TradeEnquiriesViewModel tradeEnquiriesViewModel)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    bool result = await _contactBusinessServices.SendTradeEmail(tradeEnquiriesViewModel);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Send email success", true, Request, HttpStatusCode.OK);
                    }
                    else
                    {
                        return new GenerateResponeHelper<string>("Send email failed", false, Request, HttpStatusCode.BadRequest);
                    }
                }
                //else
                //{
                //    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                //    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                //}
            }
            catch (Exception ex) {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
            
        }
        [HttpPost]
        [Route("public/api/SendContact")]
        public async Task<IHttpActionResult> SendContact(ContactRequestObj contactRequestObj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = await _contactBusinessServices.SendContactEmail(contactRequestObj);
                    if (result)
                    {
                        return new GenerateResponeHelper<string>("Send email success", true, Request, HttpStatusCode.OK);
                    }
                    else
                    {
                        return new GenerateResponeHelper<string>("Send email failed", false, Request, HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    var error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage));
                    return new GenerateResponeHelper<IEnumerable<string>>(error, false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex) {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
