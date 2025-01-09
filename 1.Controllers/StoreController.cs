using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BL.BusinessService;
using DAL.ViewModels;
using TA.Data2021.DataTransfer;
using TA.Data2021.Models;
using TA.Data2021.Repositories;
using TA_Web_2020_API.Helper;

namespace TA_Web_2020_API.Controllers
{
    //[RoutePrefix("api/store")]
    public class StoreController : TABaseAPIController
    {
        private readonly LocatorBusinessService _locatorServices;
        private readonly IStoreRepository _storeRepository;

        public StoreController(LocatorBusinessService locatorServices, IStoreRepository storeRepository)
        {
            _locatorServices = locatorServices;
            _storeRepository = storeRepository;
        }
        [HttpPost]
        public async Task<IHttpActionResult> PaginationLocaton(PaginationRequestStoreObj paginationRequestStore)
        {
            var result = await _locatorServices.PaginationLocaton(paginationRequestStore);
            return new GenerateResponeHelper<PageResult<LocatorViewModel>>(result, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetLocations()
        {
            try
            {
                var result = await _locatorServices.GetLocation();
                return new GenerateResponeHelper<IList<LocatorViewModel>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IHttpActionResult GetStoreById(Guid storeId)
        {
            try
            {
                var result = _locatorServices.GetStore(storeId);
                return new GenerateResponeHelper<LocatorViewModel>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet]
        public IHttpActionResult GetCountries()
        {
            try
            {
                var result = _locatorServices.GetSelectBoxCountries().ToList();
                return new GenerateResponeHelper<IList<CountrySelectBoxModel>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetRegionsByCountryId(string Id)
        {
            try
            {
                Guid? countryId = BL.Helper.TryParseStringToGuid(Id);
                if (countryId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invalid country", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _locatorServices.GetSelectBoxRegionsByCountryId((Guid)countryId);
                return new GenerateResponeHelper<IList<RegionSelectBoxModel>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetCitiesByRegionId(string Id)
        {
            try
            {
                Guid? regionId = BL.Helper.TryParseStringToGuid(Id);
                if (regionId == Guid.Empty)
                {
                    return new GenerateResponeHelper<string>("Invalid region", false, Request, HttpStatusCode.BadRequest);
                }
                var result = await _locatorServices.GetSelectBoxCitiesByRegionId((Guid)regionId);
                return new GenerateResponeHelper<IList<CitySelectBoxModel>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public IHttpActionResult GetCityRegionCountry(string cityName)
        {
            try
            {
                var result = _locatorServices.GetCityRegionCountry(cityName);
                return new GenerateResponeHelper<IList<CityRegionCountryModel>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public IHttpActionResult GetByUserIDToList(string userID)
        {
            try
            {
                var result = _storeRepository.GetByUserIDToList(userID);
                return new GenerateResponeHelper<IList<TA.Data2021.Models.Store>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BL.Helper.SendErrorEmail(Request, ex);
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public IHttpActionResult GetByIsActiveAndSearchStringToList(bool isActive, string searchString)
        {
            try
            {
                var result = _storeRepository.GetByIsActiveAndSearchStringToList(isActive, searchString);
                return new GenerateResponeHelper<IList<TA.Data2021.Models.Store>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public IHttpActionResult GetByFilterToList(bool isActive, string searchString, string countryISO, string regionName, decimal latitude, decimal longitude, bool isDealerNearYou)
        {
            try
            {
                var result = _storeRepository.GetByFilterToList(isActive, searchString, countryISO, regionName, latitude, longitude, isDealerNearYou);
                return new GenerateResponeHelper<IList<TA.Data2021.Models.Store>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet]
        public IHttpActionResult GetByFilter2022ToList(bool isActive, string searchString, decimal latitude, decimal longitude, decimal radius)
        {
            try
            {
                var result = _storeRepository.GetByFilter2022ToList(isActive, searchString, latitude, longitude, radius);
                return new GenerateResponeHelper<IList<TA.Data2021.Models.Store>>(result, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new GenerateResponeHelper<string>(ex.Message, false, Request, HttpStatusCode.InternalServerError);
            }
        }
    }
}
