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

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/IntiaroConfigurationImage")]
    public class IntiaroConfigurationImageController : TABaseAPIController
    {
        private readonly IIntiaroConfigurationImageRepository _intiaroConfigurationImageRepository;       
        public IntiaroConfigurationImageController(IIntiaroConfigurationImageRepository intiaroConfigurationImageRepository
           
            ) : base()
        {
            _intiaroConfigurationImageRepository = intiaroConfigurationImageRepository;
           
        }

        [HttpGet]
        public IHttpActionResult GetByUserIDToList(string userID)
        {
            List<TA.Data2021.Models.IntiaroConfigurationImage> result = _intiaroConfigurationImageRepository.GetByUserIDToList(userID);
            return new GenerateResponeHelper<List<TA.Data2021.Models.IntiaroConfigurationImage>>(result, true, Request, HttpStatusCode.OK);
        }

      
    }
}
