using BL.BUServices;
using BL.DTO;
using DAL.ViewModels;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TA.Data2021.Models;
using TA.Data2021.Repositories;
using TA_Web_2020_API.Helper;
using TA_Web_2020_API.ViewModel;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/shoppingcart")]
    public class ShoppingCartController : TABaseAPIController
    {
        private readonly IOrderService _orderService;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShoppingCart_ItemRepository _shoppingCart_ItemRepository;

        private readonly IIntiaroConfigurationRepository _intiaroConfigurationRepository;

        public ShoppingCartController(IOrderService orderService
            , IShoppingCartRepository shoppingCartRepository
            , IShoppingCart_ItemRepository shoppingCart_ItemRepository
            , IIntiaroConfigurationRepository intiaroConfigurationRepository
        )
        {
            _orderService = orderService;
            _shoppingCartRepository = shoppingCartRepository;
            _shoppingCart_ItemRepository = shoppingCart_ItemRepository;
            _intiaroConfigurationRepository = intiaroConfigurationRepository;
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateShoppingCart(BL.DTO.AddToCardRequestObj addToCardRequestObj)
        {
            if (!ModelState.IsValid)
            {
                return new GenerateResponeHelper<string>("Invailid request", false, Request, HttpStatusCode.BadRequest);
            }
            var jwtModel = await _helper.GenerateJWTViewModel();
            if (jwtModel.UserId == Guid.Empty)
            {
                return new GenerateResponeHelper<string>("Update shopping cart failed: Invailid User", false, Request, HttpStatusCode.BadRequest);
            }

            string errorMessage = String.Empty;
            try
            {
                var result = _orderService.UpdateShoppingCart(addToCardRequestObj, jwtModel, out errorMessage);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    return new GenerateResponeHelper<BL.DTO.ShoppingCartViewModel>(result, true, Request, HttpStatusCode.OK);
                }
                else
                {
                    return new GenerateResponeHelper<string>(errorMessage, false, Request, HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(String.Format("Error: {0}: {1}", ex.Message, errorMessage), false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> RemoveItemInShoppingCart(string Id)
        {
            try
            {
                var ItemItd = BL.Helper.TryParseStringToGuid(Id);
                if (ItemItd == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Remove item in shopping cart failed", false, Request, HttpStatusCode.BadRequest);
                }
                else
                {
                    var jwtModel = await _helper.GenerateJWTViewModel();
                    if (jwtModel.UserId == Guid.Empty)
                    {
                        return new GenerateResponeHelper<string>("Remove item in shopping cart failed, Invailid User", false, Request, HttpStatusCode.BadRequest);
                    }
                    var result = await _orderService.RemoveItemInShoppingCart((Guid)ItemItd, jwtModel);
                    return new GenerateResponeHelper<BL.DTO.ShoppingCartViewModel>(result, true, Request, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        //getshopping cart count
        //TODO

        [HttpGet]
        public async Task<IHttpActionResult> GetShoppingCartItem()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Get items in shopping cart failed, Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                var result = _orderService.GetShoppingCartByUserId(jwtModel);
                return new GenerateResponeHelper<BL.DTO.ShoppingCartViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> CheckOutShoppingCart()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Check out shopping cart failed, Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                var cart = await _orderService.CheckOutShoppingCart(jwtModel);
                return new GenerateResponeHelper<BL.DTO.AvailabilityChangedShoppingCart>(cart, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetInfosToCreateNewOrder()
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                var result = new List<OrderDto> { _orderService.GetInfosToCreateNewOrder(jwtModel) }.AsQueryable().Select(OrderViewModel.Dto2ViewModelSelector).FirstOrDefault();
                return new GenerateResponeHelper<OrderViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public IHttpActionResult GetByUserIDAndIsActiveAndSearchString(string userID, bool isActive, string searchString)
        {
            try
            {
                var result = _shoppingCartRepository.GetByUserIDAndIsActiveAndSearchString(userID, isActive, searchString);
                return new GenerateResponeHelper<List<ShoppingCart>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public IHttpActionResult GetShoppingCart_ItemByUserIDAndIsActive(string userID, bool isActive)
        {
            try
            {
                var result = _shoppingCart_ItemRepository.GetByUserIDAndIsActive(userID, isActive);
                return new GenerateResponeHelper<List<ShoppingCart_Item>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Error: " + ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SubmitOrder(OrderViewModel orderUpdateModel)
        {
            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                if (string.IsNullOrEmpty(orderUpdateModel.StoreID))
                {
                    orderUpdateModel.StoreID = "";
                }
                if (string.IsNullOrEmpty(orderUpdateModel.TausID))
                {
                    orderUpdateModel.TausID = "";
                }
                OrderDto orderDto = new List<OrderViewModel> { orderUpdateModel }.AsQueryable().Select(OrderViewModel.ViewModel2DtoSelector).FirstOrDefault();
                orderDto = await _orderService.SubmitOrder(orderDto, jwtModel);
                OrderViewModel resViewModel = new List<OrderDto> { orderDto }.AsQueryable().Select(OrderViewModel.Dto2ViewModelSelector).FirstOrDefault();
                if (orderDto != null)
                {
                    if (orderDto.ShoppingCartId != null)
                    {
                        ShoppingCart shoppingCart = _shoppingCartRepository.GetByID(orderDto.ShoppingCartId.ToString());
                        if (shoppingCart != null)
                        {
                            List<ShoppingCart_Item> listShoppingCart_Item = _shoppingCart_ItemRepository.GetByShoppingCart_IDToList(orderDto.ShoppingCartId.ToString());

                            EmailViewModel email = new EmailViewModel();

                            string fileName = "Order.html";
                            string subPath = "Download/HTML";
                            var physicalPathRead = Path.Combine(TA.Helpers2021.AppGlobal.WebRootPath, subPath, fileName);
                            using (FileStream fs = new FileStream(physicalPathRead, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    email.Body = r.ReadToEnd();
                                }
                            }
                            if (string.IsNullOrEmpty(shoppingCart.StoreName))
                            {
                                shoppingCart.StoreName = TA.Helpers2021.AppGlobal.InitializationString;
                            }

                            email.Subject = "Order code #" + shoppingCart.Code + " by " + shoppingCart.Email + " at " + shoppingCart.OrderDate;
                            email.Body = email.Body.Replace(@"[Code]", shoppingCart.Code);
                            email.Body = email.Body.Replace(@"[ItemCount]", shoppingCart.ItemCount);
                            email.Body = email.Body.Replace(@"[Total]", shoppingCart.Total);
                            email.Body = email.Body.Replace(@"[Volume]", shoppingCart.Volume);
                            email.Body = email.Body.Replace(@"[FirstName]", shoppingCart.FirstName);
                            email.Body = email.Body.Replace(@"[LastName]", shoppingCart.LastName);
                            email.Body = email.Body.Replace(@"[StoreName]", shoppingCart.StoreName);
                            email.Body = email.Body.Replace(@"[TAUSID]", shoppingCart.AccountNumber);
                            email.Body = email.Body.Replace(@"[UserName]", shoppingCart.UserName);
                            email.Body = email.Body.Replace(@"[UserTypeName]", shoppingCart.UserTypeName);
                            email.Body = email.Body.Replace(@"[AccountNumber]", shoppingCart.AccountNumber);
                            if (shoppingCart.OrderDate != null)
                            {
                                try
                                {
                                    email.Body = email.Body.Replace(@"[OrderDate]", shoppingCart.OrderDate.Value.ToString("MM/dd/yyyy HH:mm:ss tt"));
                                }
                                catch (Exception e)
                                {
                                    string mes = e.Message;
                                    email.Body = email.Body.Replace(@"[OrderDate]", TA.Helpers2021.AppGlobal.InitializationDateTime.ToString("MM/dd/yyyy HH:mm:ss tt"));
                                }
                            }
                            else
                            {
                                email.Body = email.Body.Replace(@"[OrderDate]", TA.Helpers2021.AppGlobal.InitializationDateTime.ToString("MM/dd/yyyy HH:mm:ss tt"));
                            }
                            email.Body = email.Body.Replace(@"[ShippingDate]", shoppingCart.ShippingDate);
                            email.Body = email.Body.Replace(@"[Email]", shoppingCart.Email);
                            email.Body = email.Body.Replace(@"[BillingAddressString]", shoppingCart.BillingAddressString);
                            email.Body = email.Body.Replace(@"[ShippingAddressString]", shoppingCart.ShippingAddressString);
                            email.Body = email.Body.Replace(@"[ContainerReference]", shoppingCart.ContainerReference);
                            email.Body = email.Body.Replace(@"[SpecialInstruction]", shoppingCart.SpecialInstruction);

                            StringBuilder orderDetail = new StringBuilder();
                            foreach (TA.Data2021.Models.ShoppingCart_Item item in listShoppingCart_Item)
                            {
                                try
                                {
                                    orderDetail.AppendLine(@"<tr>");
                                    orderDetail.AppendLine(@"<td style='border-bottom: 1px solid #e4e4e4;'>");
                                    orderDetail.AppendLine(@"<table id='product_ta'>");
                                    orderDetail.AppendLine(@"<tr style='border-bottom:1px solid #e4e4e4'>");
                                    orderDetail.AppendLine(@"<td style='width:40%'>");
                                    orderDetail.AppendLine(@"<a href='" + item.URL + "' target='_blank' title='" + item.ProductName + "'><img title='" + item.ProductName + "' alt='" + item.ProductName + "' src='" + item.ImageURL + "?w=200&h=200' /></a>");
                                    orderDetail.AppendLine(@"</td>");
                                    orderDetail.AppendLine(@"<td style='text-align:left'>");
                                    orderDetail.AppendLine(@"<p style='font-weight: bold; font-size: 20px;'>");
                                    orderDetail.AppendLine(@"<a style='text-decoration: none; color:#212121' href='" + item.URL + "' target='_blank' title='" + item.ProductName + "'>" + item.ProductName + "</a>");
                                    orderDetail.AppendLine(@"</p>");
                                    orderDetail.AppendLine(@"<p><a style='text-decoration: none; color:#212121' href='" + item.URL + "' target='_blank' title='" + item.ProductName + "'>" + item.SKU + "</a></p>");
                                    orderDetail.AppendLine(@"<p>" + item.Volume + " CBM</p>");
                                    if (item.DealerPrice != null)
                                    {
                                        orderDetail.AppendLine(@"<p>" + item.Quantity + " x " + item.DealerPrice + " = " + (item.Quantity * item.DealerPrice).Value.ToString("C0") + "</p>");
                                    }
                                    else
                                    {
                                        if (item.DesignerPrice != null)
                                        {
                                            orderDetail.AppendLine(@"<p>" + item.Quantity + " x " + item.DesignerPrice + " = " + (item.Quantity * item.DesignerPrice).Value.ToString("C0") + "</p>");
                                        }
                                        else
                                        {
                                            orderDetail.AppendLine(@"<p>" + item.Quantity + " x " + item.Price + " = " + (item.Quantity * item.Price).Value.ToString("C0") + "</p>");
                                        }
                                    }

                                    orderDetail.AppendLine(@"<p>Availability: " + item.Availability + "</p>");
                                    orderDetail.AppendLine(@"</td>");
                                    orderDetail.AppendLine(@"</tr>");
                                    orderDetail.AppendLine(@"</table>");
                                    orderDetail.AppendLine(@"</td>");
                                    orderDetail.AppendLine(@"</tr>");
                                }
                                catch (Exception e)
                                {
                                    string mes = e.Message;
                                }
                            }
                            email.Body = email.Body.Replace(@"[OrderDetail]", orderDetail.ToString());

                            email.MailTo = shoppingCart.Email;
                            await BL.Helper.SendMail2022(email.Display, email.MailTo, email.Subject, email.Body);

                            //email.MailTo = "wjohan@theodorealexander.com";
                            //await BL.Helper.SendMail2022(email.Display, email.MailTo, email.Subject, email.Body);

                            email.MailTo = "mlduong@theodorealexander.com";
                            await BL.Helper.SendMail2022(email.Display, email.MailTo, email.Subject, email.Body);

                            if (string.IsNullOrEmpty(orderUpdateModel.Region))
                            {
                                //email.MailTo = "CSD_TAUS@theodorealexander.com";
                                //await BL.Helper.SendMail2022(email.Display, email.MailTo, email.Subject, email.Body);

                                //email.MailTo = "csd@theodorealexander.com";
                                //await BL.Helper.SendMail2022(email.Display, email.MailTo, email.Subject, email.Body);
                            }
                            else
                            {
                                if (orderUpdateModel.Region.ToLower() == "taus")
                                {
                                    //email.MailTo = "CSD_TAUS@theodorealexander.com";
                                    //await BL.Helper.SendMail2022(email.Display, email.MailTo, email.Subject, email.Body);
                                }
                                else
                                {
                                    //email.MailTo = "csd@theodorealexander.com";
                                    //await BL.Helper.SendMail2022(email.Display, email.MailTo, email.Subject, email.Body);
                                }
                            }
                        }
                    }
                }
                return new GenerateResponeHelper<OrderViewModel>(resViewModel, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<OrderViewModel>(orderUpdateModel, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public IHttpActionResult SaveIntiaroConfiguration(IntiaroConfigurationViewModel model)
        {
            try
            {
                return new GenerateResponeHelper<IntiaroConfigurationViewModel>(model, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                model.CodeManage = ex.ToString();
                return new GenerateResponeHelper<IntiaroConfigurationViewModel>(model, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> SubmitOrder2021()
        {
            OrderViewModel orderUpdateModel = new OrderViewModel();
            orderUpdateModel.SpecialInstruction = "We will be testing order emails today. If an order has “TEST ORDER DO NOT PROCESS” in the notes, please know that it is not an actual order.";
            orderUpdateModel.ContainerReference = "";
            orderUpdateModel.BillingAddressString = "dasd, dsad, Bulqize, Qarku i Dibres, Albania, fdsfsd, Phone: fsdf";
            orderUpdateModel.OrderBy = null;
            orderUpdateModel.OrderDate = DateTime.Parse("0001-01-01T00:00:00");
            orderUpdateModel.ShippingAddressString = "dasd, dsad, Bulqize, Qarku i Dibres, Albania, fdsfsd, Phone: fsdf";
            orderUpdateModel.ShippingDate = null;
            orderUpdateModel.ShoppingCartId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            orderUpdateModel.StoreID = "0a696fbc-b749-4b61-8c2a-11379522b088";
            orderUpdateModel.ShoppingCartInfos = new BL.DTO.ShoppingCartViewModel();
            orderUpdateModel.ShoppingCartInfos.Count = "2";
            orderUpdateModel.ShoppingCartInfos.Total = "1290.00000000";
            orderUpdateModel.ShoppingCartInfos.Volume = "0.75";

            try
            {
                var jwtModel = await _helper.GenerateJWTViewModel();
                jwtModel.UserId = Guid.Parse("807c03fc-9e15-4200-a7f1-bfae48128cb0");
                if (jwtModel.UserId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invailid User", false, Request, HttpStatusCode.BadRequest);
                }
                OrderDto orderDto = new List<OrderViewModel> { orderUpdateModel }.AsQueryable().Select(OrderViewModel.ViewModel2DtoSelector).FirstOrDefault();
                orderDto = await _orderService.SubmitOrder(orderDto, jwtModel);
                OrderViewModel resViewModel = new List<OrderDto> { orderDto }.AsQueryable().Select(OrderViewModel.Dto2ViewModelSelector).FirstOrDefault();
                return new GenerateResponeHelper<OrderViewModel>(resViewModel, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<OrderViewModel>(orderUpdateModel, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetSendMail(EmailViewModel model)
        {
            try
            {
                await BL.Helper.SendMail2022(model.Display, model.MailTo, model.Subject, model.Body);
                return new GenerateResponeHelper<string>("OK", true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Erro", false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> PostSendMail(EmailViewModel model)
        {
            try
            {
                await BL.Helper.SendMail2022(model.Display, model.MailTo, model.Subject, model.Body);
                return new GenerateResponeHelper<string>("OK", true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>("Erro", false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
