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
using TA.Data2021.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Text;
using TA_Web_2020_API.ViewModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Org.BouncyCastle.Utilities.Collections;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/item")]
    public class ItemController : TABaseAPIController
    {
        private readonly IContactService _contactService;
        private readonly IProductDetailItemService _productDetailItemService;
        private readonly IProductListingItemService _productListingItemService;

        private readonly IItemRepository _itemRepository;

        public ItemController(IContactService contactService
            , IProductDetailItemService productDetailItemService
            , IProductListingItemService productListingItemService
            , IItemRepository itemRepository
            )
        {
            _contactService = contactService;
            _productDetailItemService = productDetailItemService;
            _productListingItemService = productListingItemService;
            _itemRepository = itemRepository;
            Initialization();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetColourAndFinishs()
        {
            try
            {
                //var result = await _itemServices.GetColourAndFinishes();
                var result = await _productListingItemService.GetColourAndFinishes();
                return new GenerateResponeHelper<IList<ColourAndFinishViewModel>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Cannot get colour and finish: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetItemsV2(RequestItemObj obj)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();

                var result = Task.Run(() =>
                {
                    return _productListingItemService.GetListingItemsV2(obj, jwtModel);
                });

                var items = await result;
                return new GenerateResponeHelper<PageResult<ItemDto>>(items, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(obj));
                return new GenerateResponeHelper<string>("Not found: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> GetItems2021()
        {
            RequestItemObj obj = new RequestItemObj();
            obj.DefaultItemOnly = true;
            obj.OrderBy = "IsNew";
            obj.PageNum = 1;
            obj.SearchKey = "iconic";
            obj.NewOnly = false;            
            obj.CollectionIds = "";
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var items = _productListingItemService.GetListingItemsV2(obj, jwtModel);
                return new GenerateResponeHelper<PageResult<ItemDto>>(items, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(obj));
                return new GenerateResponeHelper<string>("Not found: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> GetDimensionRange(DimensionRequestObj obj)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                DimensionCMAndInch ranges = await _productListingItemService.GetItemRanges(obj, jwtModel);
                return new GenerateResponeHelper<DimensionCMAndInch>(ranges, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(obj));
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetItemById(string Id)//ID or SKU
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var guidId = BL.Helper.TryParseStringToGuid(Id);
                ItemDto item = await _productDetailItemService.GetItemByID(jwtModel, guidId);
                if (item == null)
                {
                    return new GenerateResponeHelper<string>("Not Founds", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<ItemDto>(item, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetItemBySku(string sku)//ID or SKU
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                ItemDto item = await _productDetailItemService.GetItemBySKU(jwtModel, sku);
                if (item == null)
                {
                    return new GenerateResponeHelper<string>("Not Founds", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<ItemDto>(item, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetItemByCollectionId(string collectionId)//ID or SKU
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var item = await _productDetailItemService.GetItemByCollectionId(collectionId);
                if (item == null)
                {
                    return new GenerateResponeHelper<string>("Not Founds", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<List<BarstoolItem>>(item, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetItemByURLCode(string URLCode)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                ItemDto item = await _productDetailItemService.GetItemByURLCode(jwtModel, URLCode);
                if (item == null)
                {
                    return new GenerateResponeHelper<string>("Not Founds", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<ItemDto>(item, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetRelatedItemByURLCode(string URLCode)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var item = await _productDetailItemService.GetRelatedItemByURLCode(jwtModel, URLCode);
                if (item == null)
                {
                    return new GenerateResponeHelper<string>("Not Founds", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<List<RelatedItemViewModel>>(item, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetOptionsItemByURLCode(string URLCode)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                ItemDto item = await _productDetailItemService.GetOptionsItemByURLCode(jwtModel, URLCode);
                if (item == null)
                {
                    return new GenerateResponeHelper<string>("Not Founds", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<ItemDto>(item, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetDownloadItemByURLCode(string URLCode)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var item = await _productDetailItemService.GetDownloadItemByURLCode(jwtModel, URLCode);
                if (item == null)
                {
                    return new GenerateResponeHelper<string>("Not Founds", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<List<Downloads>>(item, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetImagesItemByURLCode(string URLCode)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var item = await _productDetailItemService.GetImagesItemByURLCode(jwtModel, URLCode);
                if (item == null)
                {
                    return new GenerateResponeHelper<string>("Not Founds", false, Request, HttpStatusCode.NotFound);
                }
                return new GenerateResponeHelper<List<ItemImages>>(item, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> CountItemForSideBarFilter(CountItemForSidebarRequestObj requestObj)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                jwtModel.Region = requestObj.IncludeCountby.Region;

                var task = Task.Run(() =>
                {
                    return _productListingItemService.CountItemForSidebarFilter(requestObj, jwtModel);
                });

                SidebarItemCountViewModel result = await task;
                return new GenerateResponeHelper<SidebarItemCountViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(requestObj));
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SendEmailItem(RequestSendEmailItemObj obj)
        {
            try
            {
                if (String.IsNullOrEmpty(obj.ItemId))
                {
                    return new GenerateResponeHelper<string>("Invalid item", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _contactService.SendEmailItem(obj);
                return new GenerateResponeHelper<string>(result ? "Send email success" : "Send email failed", result, Request, result ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                //BL.Helper.SendErrorEmail(Request, ex, new JavaScriptSerializer().Serialize(obj));
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetByURLCode(string URLCode)
        {
            TA.Data2021.Models.Item item = _itemRepository.GetByURLCode(URLCode);
            return new GenerateResponeHelper<TA.Data2021.Models.Item>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetBySKU(string SKU)
        {
            TA.Data2021.Models.Item item = _itemRepository.GetBySKU(SKU);
            return new GenerateResponeHelper<TA.Data2021.Models.Item>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByID(string ID)
        {
            TA.Data2021.Models.Item item = _itemRepository.GetByID(ID);
            return new GenerateResponeHelper<TA.Data2021.Models.Item>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndIsActiveTAUSAndType_IDAndInStockProgramAndIsStockedToList(string user_ID, bool isActiveTAUS, string type_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndType_IDAndInStockProgramAndIsStockedToList(user_ID, isActiveTAUS, type_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionSAndType_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string type_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndType_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), type_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionAndShape_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string shape_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndShape_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), shape_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionAndRoomAndUsage_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string roomAndUsage_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndRoomAndUsage_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), roomAndUsage_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionAndLifeStyle_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string lifeStyle_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndLifeStyle_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), lifeStyle_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionAndCollection_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string collection_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndCollection_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), collection_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionAndBrand_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string brand_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndBrand_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), brand_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionAndExtendingAndInStockProgramAndIsStockedToList(string user_ID, string region, bool extending, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndExtendingAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), extending, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionAndSearchStringAndInStockProgramAndIsStockedToList(string user_ID, string region, string searchString, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndSearchStringAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), searchString, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }

        [HttpGet]
        public IHttpActionResult GetByRegionAndExtendingToItemCount(string region, bool extending)
        {
            int result = AppGlobal.InitializationNumber;
            string path = AppGlobal.WebRootPath + AppGlobal.Download + AppGlobal.RightCross + AppGlobal.Menu + AppGlobal.RightCross + AppGlobal.ItemDataTransferActive + AppGlobal.International + AppGlobal.Extending + AppGlobal.JSONExtension;
            if (region.ToLower() == AppGlobal.TAUS.ToLower())
            {
                path = AppGlobal.WebRootPath + AppGlobal.Download + AppGlobal.RightCross + AppGlobal.Menu + AppGlobal.RightCross + AppGlobal.ItemDataTransferActive + AppGlobal.TAUS + AppGlobal.Extending + AppGlobal.JSONExtension;
            }
            string json = AppGlobal.InitializationString;
            using (StreamReader read = new StreamReader(path))
            {
                json = read.ReadToEnd();
            }
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = JsonConvert.DeserializeObject<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(json);
            result = list.Count;
            return new GenerateResponeHelper<int>(result, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByIDListToList(string IDList)
        {
            string path = AppGlobal.WebRootPath + AppGlobal.Download + AppGlobal.RightCross + AppGlobal.Menu + AppGlobal.RightCross + AppGlobal.ItemActive + AppGlobal.JSONExtension;
            string json = AppGlobal.InitializationString;
            List<Item> result = new List<Item>();
            try
            {
                using (StreamReader read = new StreamReader(path))
                {
                    json = read.ReadToEnd();
                }
                List<TA.Data2021.Models.Item> list = JsonConvert.DeserializeObject<List<TA.Data2021.Models.Item>>(json);
               
                int number = AppGlobal.InitializationNumber;
                foreach (string IDString in IDList.Split(';'))
                {
                    if (!string.IsNullOrEmpty(IDString))
                    {
                        number = number + 1;
                        try
                        {
                            Item item = list.Find(model => model.ID == new Guid(IDString));
                            if (item != null)
                            {
                                result.Add(item);
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                if (result.Count != number)
                {
                    CreateMenuList();
                    using (StreamReader read = new StreamReader(path))
                    {
                        json = read.ReadToEnd();
                    }
                    list = JsonConvert.DeserializeObject<List<TA.Data2021.Models.Item>>(json);
                    result = new List<Item>();
                    foreach (string IDString in IDList.Split(';'))
                    {
                        if (!string.IsNullOrEmpty(IDString))
                        {
                            try
                            {
                                Item item = list.Find(model => model.ID == new Guid(IDString));
                                if (item != null)
                                {
                                    result.Add(item);
                                }
                            }
                            catch
                            {

                            }
                        }
                    }

                }
                return new GenerateResponeHelper<List<TA.Data2021.Models.Item>>(result, true, Request, HttpStatusCode.OK);
            }
            catch
            {
                return new GenerateResponeHelper<List<TA.Data2021.Models.Item>>(result, true, Request, HttpStatusCode.OK);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndType_IDAndInStockProgramAndIsStockedToList(string user_ID, bool isActiveTAUS, string type_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndType_IDAndInStockProgramAndIsStockedToList(user_ID, isActiveTAUS, type_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndType_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string type_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndType_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), type_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndShape_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string shape_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndShape_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), shape_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndRoomAndUsage_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string roomAndUsage_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndRoomAndUsage_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), roomAndUsage_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndLifeStyle_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string lifeStyle_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndLifeStyle_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), lifeStyle_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndStyle_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string style_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndStyle_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), style_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndCollection_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string collection_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndCollection_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), collection_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndBrand_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string brand_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndBrand_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), brand_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndExtendingAndInStockProgramAndIsStockedToList(string user_ID, string region, bool extending, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndExtendingAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), extending, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndSearchStringAndInStockProgramAndIsStockedToList(string user_ID, string region, string searchString, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndSearchStringAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), searchString, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndInStockProgramAndIsStockedToList(string user_ID, string region, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndCasualLivingAndInStockProgramAndIsStockedToList(string user_ID, string region, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndCasualLivingAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndIsCFPItemAndInStockProgramAndIsStockedToList(string user_ID, string region, bool isCFPItem, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndIsCFPItemAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), isCFPItem, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndOptionGroup2_IDAndInStockProgramAndIsStockedToList(string user_ID, string region, string optionGroup2_ID, bool inStockProgram, bool isStocked)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndOptionGroup2_IDAndInStockProgramAndIsStockedToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), optionGroup2_ID, inStockProgram, isStocked);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndRegionAndFiltersToList(string user_ID, string region, string room_IDList, string brand_IDList, string type_IDList, string shape_IDList, string style_IDList, string lifeStyle_IDList, string collection_IDList, string designer_IDList, string searchString, bool extending, bool isStocked, bool inStockProgram = false, bool isCFPItem = false, bool isNew = false)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer001> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndFiltersToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), room_IDList, brand_IDList, type_IDList, shape_IDList, style_IDList, lifeStyle_IDList, collection_IDList, designer_IDList, searchString, extending, isStocked, inStockProgram, isCFPItem, isNew);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer001>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetDataTransferByUser_IDAndRegionAndFiltersToList(string user_ID, string region, string room_IDList, string brand_IDList, string type_IDList, string shape_IDList, string style_IDList, string lifeStyle_IDList, string collection_IDList, string designer_IDList, string searchString, bool extending, bool isStocked, bool isCFPItem, bool isNew)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer001> list = _itemRepository.GetDataTransferByUser_IDAndIsActiveTAUSAndFiltersToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), room_IDList, brand_IDList, type_IDList, shape_IDList, style_IDList, lifeStyle_IDList, collection_IDList, designer_IDList, searchString, extending, isStocked, isCFPItem, isNew);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer001>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndTailorFitProgramToList(string user_ID, string region, string room_IDList, string brand_IDList, string type_IDList, string shape_IDList, string style_IDList, string lifeStyle_IDList, string collection_IDList, string designer_IDList, string searchString, bool extending, bool isStocked, bool isCFPItem, bool isNew)
        {
            List<TA.Data2021.DataTransfer.ItemDataTransfer001> list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndTailorFitProgramToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), room_IDList, brand_IDList, type_IDList, shape_IDList, style_IDList, lifeStyle_IDList, collection_IDList, designer_IDList, searchString, extending, isStocked, isCFPItem, isNew);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer001>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetItemMenuLeftDataTransferByUser_IDAndIsActiveTAUSAndFiltersToList(string user_ID, string region, string room_IDList, string brand_IDList, string type_IDList, string shape_IDList, string style_IDList, string lifeStyle_IDList, string collection_IDList, string designer_IDList, string searchString, bool extending, bool isStocked, bool isCFPItem)
        {
            List<TA.Data2021.DataTransfer.ItemMenuLeftDataTransfer> list = await _itemRepository.AsyncGetItemMenuLeftDataTransferByUser_IDAndIsActiveTAUSAndFiltersToList(user_ID, AppGlobal.InitializationIsActiveTAUS(region), room_IDList, brand_IDList, type_IDList, shape_IDList, style_IDList, lifeStyle_IDList, collection_IDList, designer_IDList, searchString, extending, isStocked, isCFPItem);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemMenuLeftDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetByRegionAndExtendingToItemCount(string region, bool extending)
        {
            int result = await _itemRepository.AsyncGetByIsActiveTAUSAndExtendingToItemCount(AppGlobal.InitializationIsActiveTAUS(region), extending);
            return new GenerateResponeHelper<int>(result, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult InitializationItem()
        {
            return new GenerateResponeHelper<int>(1, true, Request, HttpStatusCode.OK);
        }
        private void Initialization()
        {
            //_itemRepository.UpdateItemsURLCode();
            //_itemRepository.UpdateItemsDescription();
            //DownloadItemImagesWithImageIsNull();
        }

        private void DownloadItemImagesWithImageIsNull()
        {
            string result = AppGlobal.InitializationString;
            List<Item> list = _itemRepository.GetWithImageIsNullToList();
            foreach (Item item in list)
            {
                string fileName = item.URLCode + ".jpg";
                if (!string.IsNullOrEmpty(fileName))
                {
                    string subPath = @"Images\Item";
                    try
                    {
                        var physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
                        using (WebClient client = new WebClient())
                        {
                            try
                            {
                                client.DownloadFile(new Uri(item.ImageSirv), physicalPathCreate);
                                item.IsDownloadImageSirv = true;
                                item.Image = fileName;
                                _itemRepository.UpdateBySQL(item);
                            }
                            catch (Exception e)
                            {
                                result = e.Message;

                                item.IsDownloadImageSirv = true;
                                item.Image = AppGlobal.ImageTheodoreAlexander404;
                                _itemRepository.UpdateBySQL(item);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result = e.Message;
                    }
                }
            }
        }
        private void DownloadItemImagesByList(List<TA.Data2021.DataTransfer.ItemDataTransfer> list)
        {
            string result = AppGlobal.InitializationString;
            foreach (TA.Data2021.DataTransfer.ItemDataTransfer item in list)
            {
                string subPath = @"Images\Item";
                try
                {
                    var physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, item.Image);
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile(new Uri(item.ImageSirv), physicalPathCreate);
                        }
                        catch (Exception e)
                        {
                            result = e.Message;
                        }
                    }
                }
                catch (Exception e)
                {
                    result = e.Message;
                }
            }
        }
        private void DownloadItemImagesByListCheckFileExist(List<TA.Data2021.DataTransfer.ItemDataTransfer> list)
        {
            string result = AppGlobal.InitializationString;
            foreach (TA.Data2021.DataTransfer.ItemDataTransfer item in list)
            {
                string subPath = @"Images\Item";
                string physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, item.Image);
                if (File.Exists(physicalPathCreate))
                {
                }
                else
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile(new Uri(item.ImageSirv), physicalPathCreate);
                        }
                        catch (Exception e)
                        {
                            result = e.Message;
                        }
                    }
                }
                item.Image = AppGlobal.URLImageItem + item.Image;
            }
        }
        private string CreateMenuList()
        {
            string result = AppGlobal.InitializationDateTimeCode;

            string subPath = AppGlobal.Download + AppGlobal.RightCross + AppGlobal.Menu;

            string fileName = AppGlobal.ItemActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.Item> listItem = _itemRepository.GetByIsActiveToList(true);
            string json = JsonConvert.SerializeObject(listItem);
            string physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.ItemDataTransferActive + AppGlobal.International + AppGlobal.JSONExtension;
            List<TA.Data2021.DataTransfer.ItemDataTransfer> listItemDataTransfer = _itemRepository.GetDataTransferByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listItemDataTransfer);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.ItemDataTransferActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listItemDataTransfer = _itemRepository.GetDataTransferByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listItemDataTransfer);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.ItemDataTransferActive + AppGlobal.International + AppGlobal.Extending + AppGlobal.JSONExtension;
            listItemDataTransfer = _itemRepository.GetDataTransferByIsActiveTAUSAndExtendingToList(false, true);
            json = JsonConvert.SerializeObject(listItemDataTransfer);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.ItemDataTransferActive + AppGlobal.TAUS + AppGlobal.Extending + AppGlobal.JSONExtension;
            listItemDataTransfer = _itemRepository.GetDataTransferByIsActiveTAUSAndExtendingToList(true, true);
            json = JsonConvert.SerializeObject(listItemDataTransfer);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            return result;
        }
        //------------------------NEW
        [HttpPost]
        public async Task<IHttpActionResult> GetCollectionMenuLeft(CollectionMenuLeftDto request)
        {
            if (request == null) {
                return null;
            }
            List<TA.Data2021.DataTransfer.ItemMenuLeftDataTransfer> list = await _itemRepository.AsyncGetItemMenuLeftDataTransferByUser_IDAndIsActiveTAUSAndFiltersToList(request.User_ID, AppGlobal.InitializationIsActiveTAUS(request.Region), 
                request.Room_IDList, request.Brand_IDList, request.Type_IDList, request.Shape_IDList, request.Style_IDList, request.LifeStyle_IDList, request.Collection_IDList, request.Designer_IDList,
                request.SearchString, request.Extending, request.IsStocked, request.IsCFPItem);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemMenuLeftDataTransfer>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpPost]
        public async Task<IHttpActionResult> GetCollectionDataTranfer(CollectionMenuLeftDto request)
        {
            if (request == null)
            {
                return null;
            }
            var list = await _itemRepository.AsyncGetDataTransferByUser_IDAndIsActiveTAUSAndFiltersToList
                (request.User_ID, AppGlobal.InitializationIsActiveTAUS(request.Region),
                request.Room_IDList, request.Brand_IDList, request.Type_IDList, request.Shape_IDList, request.Style_IDList, request.LifeStyle_IDList, request.Collection_IDList, request.Designer_IDList,
                request.SearchString, request.Extending, request.IsStocked, false, request.IsCFPItem, request.IsNew);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.ItemDataTransfer001>>(list, true, Request, HttpStatusCode.OK);
        }

    }
}
