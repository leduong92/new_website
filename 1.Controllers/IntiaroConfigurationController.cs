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
using System.Collections;
using System.Reflection;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/IntiaroConfiguration")]
    public class IntiaroConfigurationController : TABaseAPIController
    {
        private readonly IIntiaroConfigurationRepository _intiaroConfigurationRepository;
        private readonly IIntiaroConfigurationImageRepository _intiaroConfigurationImageRepository;
        private readonly IIntiaroConfigurationAttributesRepository _intiaroConfigurationAttributesRepository;
        public IntiaroConfigurationController(IIntiaroConfigurationRepository intiaroConfigurationRepository
            , IIntiaroConfigurationImageRepository intiaroConfigurationImageRepository
            , IIntiaroConfigurationAttributesRepository intiaroConfigurationAttributesRepository
            ) : base()
        {
            _intiaroConfigurationRepository = intiaroConfigurationRepository;
            _intiaroConfigurationImageRepository = intiaroConfigurationImageRepository;
            _intiaroConfigurationAttributesRepository = intiaroConfigurationAttributesRepository;
        }

        [HttpGet]
        public IHttpActionResult GetByParentIDToList(int parentID)
        {
            List<TA.Data2021.Models.IntiaroConfiguration> list = _intiaroConfigurationRepository.GetByParentIDToList(parentID);
            return new GenerateResponeHelper<List<TA.Data2021.Models.IntiaroConfiguration>>(list, true, Request, HttpStatusCode.OK);
        }

        [HttpPost]
        public async Task<IHttpActionResult> AsyncSave()
        {
            IntiaroConfiguration intiaroConfiguration = JsonConvert.DeserializeObject<IntiaroConfiguration>(HttpContext.Current.Request.Form["IntiaroConfiguration"]);

            try
            {
                IntiaroConfiguration intiaroConfigurationCheck = _intiaroConfigurationRepository.GetByConfigurationId(intiaroConfiguration.ConfigurationId);
                if (intiaroConfigurationCheck.ID == 0)
                {
                    string mes = await _intiaroConfigurationRepository.AsyncAddBySQL(intiaroConfiguration);
                    intiaroConfiguration.CodeManage = mes;
                    if (intiaroConfiguration.CodeManage == "-1")
                    {
                        foreach (string attributes in intiaroConfiguration.AttributesString.Split(','))
                        {
                            try
                            {
                                IntiaroConfigurationAttributes intiaroConfigurationAttributes = new IntiaroConfigurationAttributes();
                                intiaroConfigurationAttributes.ConfigurationId = intiaroConfiguration.ConfigurationId;
                                intiaroConfigurationAttributes.AttributesName = attributes.Split(':')[0];
                                intiaroConfigurationAttributes.ValueName = attributes.Split(':')[1];
                                intiaroConfigurationAttributes.AttributesDisplay = intiaroConfigurationAttributes.AttributesName.Replace(@"ta_", @"");
                                intiaroConfigurationAttributes.ValueDisplay = intiaroConfigurationAttributes.ValueName.Replace(intiaroConfigurationAttributes.AttributesName + @"_", @"");
                                await _intiaroConfigurationAttributesRepository.AsyncAddBySQL(intiaroConfigurationAttributes);
                            }
                            catch
                            {
                            }
                        }

                        //for (int i = 0; i < 360; i++)
                        //{
                        //    if (i % 10 == 0)
                        //    {
                        //        IntiaroConfigurationImage intiaroConfigurationImage = new IntiaroConfigurationImage();
                        //        intiaroConfigurationImage.ConfigurationId = intiaroConfiguration.ConfigurationId;
                        //        intiaroConfigurationImage.RowVersion = i;
                        //        intiaroConfigurationImage.FrameDictionary = "https://backend.intiaro.com/360/product_version/modular/" + intiaroConfiguration.CustomerId + "/" + intiaroConfiguration.ProductSystemVersion + "/custom_config_id/" + intiaroConfiguration.ConfigurationId + "/width/1024/height/1024/angle/" + i + "/render_settings/frames_count:36,rotate_size_x:1024,rotate_size_y:1024,shadow_enabled:true,split_image:true,tile_size_x:1024,tile_size_y:1024,zoom_size_x:2048,zoom_size_y:2048/?current_angle=0";
                        //        await _intiaroConfigurationImageRepository.AsyncAddBySQL(intiaroConfigurationImage);
                        //    }
                        //}
                        if (intiaroConfiguration.FrameDictionaries.Length > 0)
                        {
                            for (int i = 0; i< intiaroConfiguration.FrameDictionaries.Length; i++)
                            {
                                IntiaroConfigurationImage intiaroConfigurationImage = new IntiaroConfigurationImage();
                                intiaroConfigurationImage.ConfigurationId = intiaroConfiguration.ConfigurationId;
                                intiaroConfigurationImage.RowVersion = i;
                                intiaroConfigurationImage.FrameDictionary = intiaroConfiguration.FrameDictionaries[i];
                                await _intiaroConfigurationImageRepository.AsyncAddBySQL(intiaroConfigurationImage);
                            }
                        }

                    }
                }
                else
                {
                    intiaroConfiguration.CodeManage = "0";
                }    
                return new GenerateResponeHelper<IntiaroConfiguration>(intiaroConfiguration, true, Request, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                intiaroConfiguration.CodeManage = ex.ToString();
                return new GenerateResponeHelper<IntiaroConfiguration>(intiaroConfiguration, false, Request, HttpStatusCode.InternalServerError);
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

        [HttpPost]
        public async Task<IHttpActionResult> IntiaroSendEmailToCustomer()
        {
            string result = AppGlobal.InitializationString;
            IntiaroConfiguration data = JsonConvert.DeserializeObject<IntiaroConfiguration>(HttpContext.Current.Request.Form["IntiaroConfiguration"]);
            string mailAddress = JsonConvert.DeserializeObject<string>(HttpContext.Current.Request.Form["mailAddress"]);
            if (!string.IsNullOrEmpty(mailAddress))
            {
                if (string.IsNullOrEmpty(data.PDF))
                {
                    return new GenerateResponeHelper<string>("ERROR", true, Request, HttpStatusCode.NoContent);
                }

                EmailViewModel email = new EmailViewModel();
                StringBuilder resultList = new StringBuilder();

                string fileName = "IntiaroEmailTemplate.html";
                string subPath = "Download/HTML";
                var physicalPathRead = Path.Combine(TA.Helpers2021.AppGlobal.WebRootPath, subPath, fileName);
                using (FileStream fs = new FileStream(physicalPathRead, FileMode.Open))
                {
                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                    {
                        email.Body = r.ReadToEnd();
                    }
                }
                
                email.Body = email.Body.Replace(@"[Customer]", mailAddress);

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        string intiaroSubPath = "Download/IntiaroPdf";
                        var index = data.PDF.LastIndexOf('/');
                        string fileNameIntiaro = data.PDF.Substring(index + 1, data.PDF.Length - (index + 1));

                        string pdfPath = Path.Combine(AppGlobal.WebRootPath, intiaroSubPath, fileNameIntiaro);
                        string pdfPathCheck = Path.Combine(AppGlobal.WebRootPath, intiaroSubPath);

                        if (!Directory.Exists(pdfPathCheck))
                        {
                            Directory.CreateDirectory(pdfPathCheck);
                            client.DownloadFile(new Uri(data.PDF), pdfPath);
                        } 
                        else
                        {
                            DirectoryInfo di = new DirectoryInfo(pdfPathCheck);
                            foreach (FileInfo file in di.GetFiles())
                            {
                                file.Delete();
                            }
                            foreach (DirectoryInfo dir in di.GetDirectories())
                            {
                                dir.Delete(true);
                            }
                            client.DownloadFile(new Uri(data.PDF), pdfPath);
                            client.Dispose();
                        }
                        email.AttachmentURLs = pdfPath.ToString();
                    }
                    catch (Exception e)
                    {
                        result = e.Message;
                        return new GenerateResponeHelper<string>("ERROR", true, Request, HttpStatusCode.NoContent);
                    }
                }
                email.Subject = "TAilor Fit configuration successfully.";
                email.MailTo = mailAddress;
                try
                {
                    await BL.Helper.SendMaiWitAttachment(email.Display, email.MailTo, email.Subject, email.Body, email.AttachmentURLs);
                }
                catch (Exception e)
                {
                    result = e.Message;
                    return new GenerateResponeHelper<string>("ERROR", true, Request, HttpStatusCode.NoContent); ;
                }
            }
            return new GenerateResponeHelper<string>("OK", true, Request, HttpStatusCode.OK);
        }
    }
}
