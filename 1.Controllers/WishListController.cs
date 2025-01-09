using BL.BUServices;
using BL.CustomExceptions;
using BL.DTO;
using DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TA_Web_2020_API.Helper;
using TA.Helpers2021;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/wishlist")]
    public class WishListController : TABaseAPIController
    {
        //private readonly IWishListServices _wishListServices;
        private readonly IWishlistlItemService _wishListService2;

        public WishListController(IWishlistlItemService wishListService2)
        {
            //_wishListServices = wishListServices;
            _wishListService2 = wishListService2;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetWishListOfUser()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invaild User", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _wishListService2.GetWishListByUserId(jwtModel);
                //var result = await _wishListServices.GetWishListByUserId(jwtModel);

                return new GenerateResponeHelper<IList<WishListViewModel>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetWishListByUserID(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    userID = AppGlobal.InitializationGUICodeEmpty;
                }
                Guid userIDGuid = new Guid(userID);
                if (userIDGuid == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invaild User", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _wishListService2.GetWishListByUserIDGuid(userIDGuid);
                return new GenerateResponeHelper<IList<WishListViewModel>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> DeleteWishList(string wishListId)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var WishListId = BL.Helper.TryParseStringToGuid(wishListId);
                if (WishListId == Guid.Empty || jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Delete failed", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    //var deleteResult = await _wishListServices.DeleteWishList(jwtModel, (Guid)BL.Helper.TryParseStringToGuid(wishListId));
                    var deleteResult = await _wishListService2.DeleteWishList(jwtModel, (Guid)BL.Helper.TryParseStringToGuid(wishListId));
                    if (deleteResult == Guid.Empty)
                    {
                        return new GenerateResponeHelper<string>("Delete failed", false, Request, HttpStatusCode.BadRequest);
                    }
                    var result = await _wishListService2.GetWishListByUserId(jwtModel);
                    //var result = await _wishListServices.GetWishListByUserId(jwtModel);
                    return new GenerateResponeHelper<IList<WishListViewModel>>(result, true, Request, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> AddWishList(CreateWishListModel createWishListModel)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Add WishList failed", false, Request, HttpStatusCode.BadRequest);
                }
                var addResult = await _wishListService2.AddWishList((Guid)jwtModel.UserId, createWishListModel);
                if (addResult == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Add WishList failed", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _wishListService2.GetWishListOfUsersById((Guid)jwtModel.UserId, addResult);
                return new GenerateResponeHelper<WishListViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> AddWishListByUserIDAndWishlistName(string userID, string wishlistName)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    userID = AppGlobal.InitializationGUICodeEmpty;
                }
                Guid userIDGuid = new Guid(userID);
                if (userIDGuid == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invaild User", false, Request, HttpStatusCode.BadRequest);
                }
                var addResult = await _wishListService2.AddWishListByUserIDAndWishlistName(userIDGuid, wishlistName);
                if (addResult == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Add WishList failed", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _wishListService2.GetWishListOfUsersById(userIDGuid, addResult);
                return new GenerateResponeHelper<WishListViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> EditWishList(EditWishListModel editWishListModel)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Edit WishList failed", false, Request, HttpStatusCode.BadRequest);
                }
                var editResult = await _wishListService2.EditWishList((Guid)jwtModel.UserId, editWishListModel);
                if (editResult == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Edit WishList failed", false, Request, HttpStatusCode.BadRequest);
                }
                return new GenerateResponeHelper<Guid>(editResult, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetWishListItems(WishListItemsRequestObj wishListItemsRequestObj)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                //if (!String.IsNullOrEmpty(wishListItemsRequestObj.WishListId))
                {
                    IList<WishListItemViewModel2> result = await _wishListService2.GetWishListItemsByWishListId(jwtModel, new Guid(wishListItemsRequestObj.WishListId));
                    return new GenerateResponeHelper<IList<WishListItemViewModel2>>(result, true, Request, HttpStatusCode.OK);
                }
                //else
                {
                    //for anonymous user : TODO
                    //IList<WishListItemViewModel2> result = _itemServices.GetItemsByItemIds(wishListItemsRequestObj.ItemIDs, jwtModel, false);
                    //return new GenerateResponeHelper<IList<WishListItemViewModel2>>(result, true, Request, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> RemoveItemInWishList(WishListItemRequestObj wishListItemRequestObj)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var WishListId = BL.Helper.TryParseStringToGuid(wishListItemRequestObj.WishListId);
                var ItemId = BL.Helper.TryParseStringToGuid(wishListItemRequestObj.ItemId);
                if (jwtModel.UserId == Guid.Empty || WishListId == Guid.Empty || ItemId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Delete item failed", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var deleteResult = await _wishListService2.DeleteItemInWishList((Guid)ItemId, (Guid)WishListId, jwtModel);
                    if (deleteResult != Guid.Empty)
                    {
                        var result = await _wishListService2.GetWishListItemsByWishListId(jwtModel, deleteResult);
                        return new GenerateResponeHelper<IList<WishListItemViewModel2>>(result, true, Request, HttpStatusCode.OK);
                    }
                    else
                    {
                        return new GenerateResponeHelper<string>("Delete item failed", false, Request, HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> AddItemToWishList(WishListItemRequestObj wishListItemRequestObj)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var WishListId = BL.Helper.TryParseStringToGuid(wishListItemRequestObj.WishListId);
                var ItemId = BL.Helper.TryParseStringToGuid(wishListItemRequestObj.ItemId);
                if (jwtModel.UserId == Guid.Empty || WishListId == Guid.Empty || ItemId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Add item failed", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var result = await _wishListService2.AddItemToWishList((Guid)ItemId, (Guid)WishListId, (Guid)jwtModel.UserId, jwtModel.CountryId);
                    if (result != Guid.Empty)
                    {
                        return new GenerateResponeHelper<Guid>(result, true, Request, HttpStatusCode.OK);
                    }
                    return new GenerateResponeHelper<string>("Add item failed", false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> AddItemToWishListByUserIDAndWishListIDAndItemID(string userID, string wishListID, string itemID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    userID = AppGlobal.InitializationGUICodeEmpty;
                }
                if (string.IsNullOrEmpty(wishListID))
                {
                    wishListID = AppGlobal.InitializationGUICodeEmpty;
                }
                if (string.IsNullOrEmpty(itemID))
                {
                    itemID = AppGlobal.InitializationGUICodeEmpty;
                }
                Guid userIDGuid = new Guid(userID);
                Guid wishListIDGuid = new Guid(wishListID);
                Guid itemIDGuid = new Guid(itemID);
                if (userIDGuid == Guid.Empty || wishListIDGuid == Guid.Empty || itemIDGuid == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Add item failed", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var result = await _wishListService2.AddItemToWishListByUserIDAndWishListIDAndItemID(userIDGuid, wishListIDGuid, itemIDGuid);
                    if (result != Guid.Empty)
                    {
                        return new GenerateResponeHelper<Guid>(result, true, Request, HttpStatusCode.OK);
                    }
                    return new GenerateResponeHelper<string>("Add item failed", false, Request, HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> EditNoteOfWishlistItem(WishListItemViewModel wishlistItem)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty || wishlistItem.ID == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Add note for item failed", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var result = await _wishListService2.EditNoteOfWishlistItem(wishlistItem);
                    if (!result)
                    {
                        return new GenerateResponeHelper<string>("Add note for item failed", false, Request, HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        return new GenerateResponeHelper<Boolean>(result, true, Request, HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> ShareWishlist(ShareWishlistRequest shareWishlistRequest)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty || shareWishlistRequest.WishlistId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invalid login user", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var result = await _wishListService2.CreateLookBookShared(jwtModel.UserId.Value, shareWishlistRequest);
                    if (result)
                    {
                        return new GenerateResponeHelper<Boolean>(result, true, Request, HttpStatusCode.OK);
                    }
                    else
                    {
                        return new GenerateResponeHelper<string>("Share wishlist failed", false, Request, HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetSharedWishlistItems(string sharedWishlistId)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (!String.IsNullOrEmpty(sharedWishlistId))
                {
                    IList<WishListItemViewModel2> result = await _wishListService2.GetItemsOfSharedWishlist(jwtModel, new Guid(sharedWishlistId));
                    return new GenerateResponeHelper<IList<WishListItemViewModel2>>(result, true, Request, HttpStatusCode.OK);
                }
                else
                {
                    return new GenerateResponeHelper<string>("Get shared wishlist items failed: ", false, Request, HttpStatusCode.BadRequest);
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
