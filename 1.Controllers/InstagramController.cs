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
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/Instagram")]

    public class InstagramController : TABaseAPIController
    {
        private readonly IInstagramRepository _instagramRepository;
        public InstagramController(IInstagramRepository instagramRepository) : base()
        {
            _instagramRepository = instagramRepository;
        }
        [HttpGet]
        public IHttpActionResult GetLastPosts()
        {
            List<TA.Data2021.Models.Instagram> result = new List<TA.Data2021.Models.Instagram>();

            try
            {
                WebClient webClient = new WebClient();
                string content = AppGlobal.InitializationString;
                content = webClient.DownloadString(AppGlobal.InstagramGraphURL);
                content = content.Trim();
                content = content.Replace(@"},{", @"~");
                int length = content.Split('~').Length;
                for (int i = 0; i < length; i++)
                {
                    string contentSub = content.Split('~')[i];
                    if (contentSub.Contains("username") == true)
                    {
                        TA.Data2021.Models.Instagram instagram = new TA.Data2021.Models.Instagram();
                        instagram.Active = true;
                        instagram.Title = "theodore_alexander_official";
                        instagram.URLTitle = "https://www.instagram.com/theodore_alexander_official/";

                        string contentSub001 = contentSub;
                        instagram.Name = contentSub001.Replace(@"""caption"":""", @"~");
                        instagram.Name = instagram.Name.Split('~')[instagram.Name.Split('~').Length - 1];
                        instagram.Name = instagram.Name.Split('"')[0];

                        if (!string.IsNullOrEmpty(instagram.Name))
                        {                
                            instagram.Name = new JsonTextReader(new StringReader($"\"{instagram.Name}\"")).ReadAsString();                            
                        }


                        contentSub001 = contentSub;
                        instagram.URLImageAPI = contentSub001.Replace(@"""media_url"":""", @"~");
                        instagram.URLImageAPI = instagram.URLImageAPI.Split('~')[instagram.URLImageAPI.Split('~').Length - 1];
                        instagram.URLImageAPI = instagram.URLImageAPI.Split('"')[0];
                        instagram.URLImageAPI = instagram.URLImageAPI.Replace(@"\/", @"/");

                        if ((instagram.URLImageAPI.Contains(".mp4")) || (instagram.URLImageAPI.Contains("https://video")))
                        {
                            instagram.Active = false;
                        }

                        contentSub001 = contentSub;
                        instagram.URL = contentSub001.Replace(@"""permalink"":""", @"~");
                        instagram.URL = instagram.URL.Split('~')[instagram.URL.Split('~').Length - 1];
                        instagram.URL = instagram.URL.Split('"')[0];
                        instagram.URL = instagram.URL.Replace(@"\/", @"/");

                        contentSub001 = contentSub;
                        instagram.DatePostString = contentSub001.Replace(@"""timestamp"":""", @"~");
                        instagram.DatePostString = instagram.DatePostString.Split('~')[instagram.DatePostString.Split('~').Length - 1];
                        instagram.DatePostString = instagram.DatePostString.Split('"')[0];

                        try
                        {
                            instagram.DatePost = DateTime.Parse(instagram.DatePostString);
                            instagram.DatePostSub = instagram.DatePost.Value.ToLongDateString();
                        }
                        catch
                        {
                            instagram.DatePostSub = instagram.DatePostString;
                        }

                        if (!string.IsNullOrEmpty(instagram.URL))
                        {
                            result.Add(instagram);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string mes = e.Message;
            }

            return new GenerateResponeHelper<List<TA.Data2021.Models.Instagram>>(result, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByActiveToList(bool active)
        {
            List<TA.Data2021.Models.Instagram> result = new List<TA.Data2021.Models.Instagram>();
            WebClient webClient = new WebClient();
            string json = webClient.DownloadString("https://theodorealexander.com/Download/Instagram.json");
            if (!string.IsNullOrEmpty(json))
            {
                result = JsonConvert.DeserializeObject<List<TA.Data2021.Models.Instagram>>(json);
            }
            return new GenerateResponeHelper<List<TA.Data2021.Models.Instagram>>(result, true, Request, HttpStatusCode.OK);
        }
    }
}
