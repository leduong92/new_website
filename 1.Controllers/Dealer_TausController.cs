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

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/Dealer_Taus")]
    public class Dealer_TausController : TABaseAPIController
    {
        private readonly IDealer_TausRepository _dealer_TausRepository;
        public Dealer_TausController(IDealer_TausRepository dealer_TausRepository) : base()
        {
            _dealer_TausRepository = dealer_TausRepository;
        }

        [HttpGet]
        public IHttpActionResult GetByUserIDToList(string userID)
        {
            var result = _dealer_TausRepository.GetByUserIDToList(userID);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.Dealer_TausDataTransfer>>(result, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> AsyncGetByUserIDToList(string userID)
        {
            var result = await _dealer_TausRepository.AsyncGetByUserIDToList(userID);
            return new GenerateResponeHelper<List<TA.Data2021.DataTransfer.Dealer_TausDataTransfer>>(result, true, Request, HttpStatusCode.OK);
        }
    }
}
