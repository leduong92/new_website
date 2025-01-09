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
using DAL;
using TA.Data2021.Models;
using TA_Web_2020_API.ViewModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;
using ServiceStack;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/IntiaroWishlist")]
    public class IntiaroWishlistController : TABaseAPIController
    {
        private readonly IIntiaroWishlistRepository _intiaroWishlistRepository;
        private readonly IIntiaroConfigurationRepository _intiaroConfigurationRepository;
        public IntiaroWishlistController(IIntiaroWishlistRepository intiaroWishlistRepository,
           IIntiaroConfigurationRepository intiaroConfigurationRepository
            ) : base()
        {
            _intiaroWishlistRepository = intiaroWishlistRepository;
            _intiaroConfigurationRepository = intiaroConfigurationRepository;


        }

        [HttpGet]
        public IHttpActionResult GetByUserID(string userID)
        {
            TA.Data2021.Models.IntiaroWishlist result = _intiaroWishlistRepository.GetByUserID(userID);
            return new GenerateResponeHelper<TA.Data2021.Models.IntiaroWishlist>(result, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> DeleteIntiaroWishList(string id)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (string.IsNullOrEmpty(id) || jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Delete failed", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    _intiaroWishlistRepository.DeleteIntiaroWishlistById(id);
                    var result = _intiaroWishlistRepository.GetByUserID(jwtModel.UserId.ToString());
                    return new GenerateResponeHelper<IntiaroWishlist>(result, true, Request, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> RemoveChildIntiaroWishlish(WishListItemsRequestObj request)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (string.IsNullOrEmpty(request.WishListId) || jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Delete failed", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var result = _intiaroWishlistRepository.DeleteChildIntiaroWishlistByIdAndParentId(request.WishListId, request.ItemIDs);
                    //var result = _intiaroConfigurationRepository.GetByParentIDToList(ParrentId.ToInt());
                    return new GenerateResponeHelper<string>(result, true, Request, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> EditIntiaroWishList(EditWishListModel request)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Edit WishList failed", false, Request, HttpStatusCode.BadRequest);
                }
                var editResult =  _intiaroWishlistRepository.EditIntiaroWishList(request.ID, request.WishListName);
                if (string.IsNullOrEmpty(editResult))
                {
                    return new GenerateResponeHelper<string>("Edit WishList failed", false, Request, HttpStatusCode.BadRequest);
                }
                return new GenerateResponeHelper<string>(editResult, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
