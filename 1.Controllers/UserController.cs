using DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TA_Web_2020_API.Helper;
using BL.CustomExceptions;
using System.Linq;
using BL.BusinessService;
using TA_Web_2020_API.TAService;
using TA_Web_2020_API.ViewModel;
using TA.Data2021.Repositories;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : TABaseAPIController
    {
        private readonly UserBusinessService _userServices;
        private readonly IUserRepository _userRepository;
        private readonly AddressTAService _addressTaService;

        public UserController(UserBusinessService userServices, IUserRepository userRepository, AddressTAService addressTaService)
        {
            _userServices = userServices;
            _userRepository = userRepository;
            _addressTaService = addressTaService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> RetrieveUsername(RetrieveUserInfoRequest retrieveUserInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userServices.RetrieveUsername(retrieveUserInfo.Email);
                    return new GenerateResponeHelper<string>(
                    result == true ? "Your Username has been retrieved. Please check your email to see it" : "Retrieve Username failed",
                    result, Request,
                    result == true ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
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
        public async Task<IHttpActionResult> RetrievePassword(RetrieveUserInfoRequest retrieveUserInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userServices.RetrieveEmail(retrieveUserInfo.Email);
                    return new GenerateResponeHelper<string>(
                    result == true ? "Your Password has been retrieved. Please check your email to see it" : "Retrieve Password failed",
                    result, Request,
                    result == true ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
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

        [HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(string userId)
        {
            try
            {
                var guidUserId = BL.Helper.TryParseStringToGuid(userId);
                if (guidUserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User Id", false, Request, HttpStatusCode.BadRequest);
                }

                var user = await _userServices.EnabledEmail(guidUserId);
                if (user == null)
                {
                    return new GenerateResponeHelper<string>("Not found", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<UserViewModel>(user, true, Request, HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            var jwtModel = await _helper.GenerateJWTViewModel();
            if (jwtModel.UserId == Guid.Empty)
            {
                return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
            }
            var result = await _userServices.GetUserInfo((Guid)jwtModel.UserId);
            TA.Data2021.Models.User user = _userRepository.GetByID(jwtModel.UserId.ToString());
            if (user != null)
            {
                result.AccountNumber = user.AccountNumber;
            }
            return new GenerateResponeHelper<UserViewModel>(result, true, Request, HttpStatusCode.OK);
        }        
        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser(UserModelModified userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jwtModel = await _helper.GenerateJWTViewModel();
                    Guid? modifiedUserId = BL.Helper.TryParseStringToGuid(userModel.ID);
                    if (modifiedUserId == Guid.Empty || jwtModel.UserId == Guid.Empty || modifiedUserId != jwtModel.UserId)
                    {
                        return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                    }
                    var updateSuccess = await _userServices.UpdateUser(userModel);
                    if (updateSuccess)
                    {
                        var userNewInfo = await _userServices.GetUserInfo((Guid)modifiedUserId);
                        TA.Data2021.Models.User user = _userRepository.GetByID(modifiedUserId.ToString());
                        if (user != null)
                        {
                            userNewInfo.AccountNumber = user.AccountNumber;
                        }
                        return new GenerateResponeHelper<UserViewModel>(userNewInfo, true, Request, HttpStatusCode.OK);
                    }
                    else
                    {
                        return new GenerateResponeHelper<string>("Update failed", false, Request, HttpStatusCode.InternalServerError);
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
        public async Task<IHttpActionResult> ChangePassword(PasswordModifiedModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jwtModel = await _helper.GenerateJWTViewModel();
                    if (jwtModel.UserId == Guid.Empty)
                    {
                        return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                    }
                    string errorMessage = String.Empty;
                    var result = _userServices.ChangePassword(model, (Guid)jwtModel.UserId, out errorMessage);
                    return new GenerateResponeHelper<string>(
                    (result == true && String.IsNullOrEmpty(errorMessage)) ? "Change password successful" : errorMessage,
                    (result == true && String.IsNullOrEmpty(errorMessage)), Request,
                    (result == true && String.IsNullOrEmpty(errorMessage)) ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
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

        [HttpGet]
        public async Task<IHttpActionResult> GetDealerInfo()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                var resultTask = Task.Run(() => _userServices.GetDealerInfo((Guid)jwtModel.UserId));
                var result = resultTask.Result;
                if (result == null)
                {
                    return new GenerateResponeHelper<string>("Invailid Dealer", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<DealerInfoViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetDealerInfo2021(string userId= "807C03FC-9E15-4200-A7F1-BFAE48128CB0")
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                jwtModel.UserId = Guid.Parse(userId);
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _userServices.GetDealerInfo((Guid)jwtModel.UserId);
                if (result == null)
                {
                    return new GenerateResponeHelper<string>("Invailid Dealer", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<DealerInfoViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUserAddressById(Guid addressId)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                return GetAddressOfUser(jwtModel.UserId.Value, addressId);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetUserAddressByUserId(Guid? userId)
        {
            try
            {
                if (userId == null || !userId.HasValue || userId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                List<AddressViewModel> ret = _addressTaService.GetAddressOfUser(userId.Value);
                return new GenerateResponeHelper<List<AddressViewModel>>(ret, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetAddressOfUser failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUserAddress()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                return GetUserAddressByUserId(jwtModel.UserId.Value);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetAddressOfUser failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetAddressOfUser(Guid userId, Guid addressId)
        {
            try
            {
                if (userId == null || userId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                AddressViewModel ret = _addressTaService.GetAddressOfUser(userId, addressId);
                return new GenerateResponeHelper<AddressViewModel>(ret, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetAddressOfUser failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetActiveUserAddress()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                return GetActiveUserAddressByUserId(jwtModel.UserId.Value);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetAddressOfUser failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetActiveUserAddressByUserId(Guid userId)
        {
            try
            {
                if (userId == null || userId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                List<AddressViewModel> ret = _addressTaService.GetActiveAddressOfUser(userId);
                return new GenerateResponeHelper<List<AddressViewModel>>(ret, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("GetAddressOfUser failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUserAddress(AddressViewModel userAddress)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel == null || jwtModel.UserId == null || !jwtModel.UserId.HasValue || jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                AddressViewModel updatedAddress = _addressTaService.UpdateUserAddress(jwtModel, userAddress);
                return new GenerateResponeHelper<AddressViewModel>(updatedAddress, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("UpdateUserAddress failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddUserAddress(AddressViewModel addressModel)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel == null || jwtModel.UserId == null || !jwtModel.UserId.HasValue || jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                var newUserAddress = _addressTaService.AddUserAddress(jwtModel, addressModel);
                return new GenerateResponeHelper<AddressViewModel>(newUserAddress, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("AddUserAddress failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> DeleteUserAddress(Guid userAddressId)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == null || !jwtModel.UserId.HasValue || jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid Data", false, Request, HttpStatusCode.BadRequest);
                }
                bool ret = _addressTaService.DeleteUserAddress(jwtModel, userAddressId);
                return new GenerateResponeHelper<bool>(ret, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("DeleteUserAddress failed: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUserSetting()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _userServices.GetUserSetting((Guid)jwtModel.UserId);
                if (result == null)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<UserSettingViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateUserSetting(UserSettingViewModel userSetting)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                var updateResult = await _userServices.UpdateUserSetting(userSetting, (Guid)jwtModel.UserId);
                if (!updateResult)
                {
                    return new GenerateResponeHelper<string>("Update user setting failed", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _userServices.GetUserSetting((Guid)jwtModel.UserId);
                if (result == null)
                {
                    return new GenerateResponeHelper<string>("Invailid Dealer", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<UserSettingViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> RevokeAssociate(RevokeAssociateRequestObj obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saleId = BL.Helper.TryParseStringToGuid(obj.SaleId);
                    var storeId = BL.Helper.TryParseStringToGuid(obj.StoreId);
                    if (saleId == Guid.Empty)
                    {
                        return new GenerateResponeHelper<string>("Invailid Sale ID", false, Request, HttpStatusCode.NotFound);
                    }
                    if (storeId == Guid.Empty)
                    {
                        return new GenerateResponeHelper<string>("Invailid Store ID", false, Request, HttpStatusCode.NotFound);
                    }
                    var result = await _userServices.RevokeAssociate((Guid)saleId, (Guid)storeId);
                    return new GenerateResponeHelper<string>(
                        result == true ? "Revoke successful" : "Revoke failed",
                        result, Request,
                        result == true ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
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
        public async Task<IHttpActionResult> ApprovedAssociate(ApprovedAssociateRequestObj obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saleId = BL.Helper.TryParseStringToGuid(obj.SaleId);
                    var storeId = BL.Helper.TryParseStringToGuid(obj.StoreId);
                    if (saleId == Guid.Empty)
                    {
                        return new GenerateResponeHelper<string>("Invailid Sale ID", false, Request, HttpStatusCode.NotFound);
                    }
                    if (storeId == Guid.Empty)
                    {
                        return new GenerateResponeHelper<string>("Invailid Store ID", false, Request, HttpStatusCode.NotFound);
                    }
                    var result = await _userServices.ApproveAssociateRequest((Guid)saleId, (Guid)storeId);
                    return new GenerateResponeHelper<string>(
                        result == true ? "Approve successful" : "Approve failed",
                        result, Request,
                        result == true ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
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
        public async Task<IHttpActionResult> UpdateDealer(DealerSettingPageUpdateModel dealerSettingPageUpdateModel)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                var updateResult = await _userServices.UpdateDealerSettingPage(dealerSettingPageUpdateModel, (Guid)jwtModel.UserId);
                if (!updateResult)
                {
                    return new GenerateResponeHelper<string>("Update dealer failed", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _userServices.GetDealerInfo((Guid)jwtModel.UserId);
                if (result == null)
                {
                    return new GenerateResponeHelper<string>("Invailid Dealer", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<DealerInfoViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
