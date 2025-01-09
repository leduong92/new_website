
using System.Threading.Tasks;
using System.Web.Http;
using TA_Web_2020_API.Helper;
using BL.DTO;
using BL.BUServices;
using System.Web.Script.Serialization;
using TA.Data2021.Repositories;
using TA.Helpers2021;
using TA.Data2021.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System;

namespace TA_Web_2020_API.Controllers
{

    public class DownloadController : TABaseAPIController
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IDesignerRepository _designerRepository;
        private readonly ILifeStyleRepository _lifeStyleRepository;
        private readonly IStyleRepository _styleRepository;
        private readonly IRoomAndUsageRepository _roomAndUsageRepository;
        private readonly IShapeRepository _shapeRepository;
        private readonly IItemRepository _itemRepository;
        public DownloadController(ICollectionRepository collectionRepository
            , ITypeRepository typeRepository
            , IBrandRepository brandRepository
            , IDesignerRepository designerRepository
            , ILifeStyleRepository lifeStyleRepository
            , IStyleRepository styleRepository
            , IRoomAndUsageRepository roomAndUsageRepository
            , IShapeRepository shapeRepository
            , IItemRepository itemRepository
        ) : base()
        {
            _collectionRepository = collectionRepository;
            _typeRepository = typeRepository;
            _brandRepository = brandRepository;
            _designerRepository = designerRepository;
            _lifeStyleRepository = lifeStyleRepository;
            _styleRepository = styleRepository;
            _roomAndUsageRepository = roomAndUsageRepository;
            _shapeRepository = shapeRepository;
            _itemRepository = itemRepository;
            Initialization();
        }
        [HttpGet]
        public IHttpActionResult GetTheodoreAlexander_NewSQLServerConectionString()
        {
            return new GenerateResponeHelper<string>(AppGlobal.TheodoreAlexander_NewSQLServerConectionString, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetWebRootPath()
        {
            return new GenerateResponeHelper<string>(AppGlobal.WebRootPath, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<string> CreateMenuList()
        {
            string result = AppGlobal.InitializationDateTimeCode;

            string subPath = AppGlobal.Download + AppGlobal.RightCross + AppGlobal.Menu;

            string fileName = AppGlobal.CollectionActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.Collection> listCollection = await _collectionRepository.GetByIsActiveToList(true);
            var json = JsonConvert.SerializeObject(listCollection);
            var physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.CollectionActive + AppGlobal.International + AppGlobal.JSONExtension;
            listCollection = _collectionRepository.GetByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listCollection);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.CollectionActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listCollection = _collectionRepository.GetByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listCollection);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.CollectionActive + AppGlobal.TheodoreAlexanderBrandID + AppGlobal.JSONExtension;
            listCollection = await _collectionRepository.GetByBrand_IDAndIsActiveToList(AppGlobal.TheodoreAlexanderBrandID, true);
            json = JsonConvert.SerializeObject(listCollection);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.CollectionActive + AppGlobal.TAStudioBrandID + AppGlobal.JSONExtension;
            listCollection = await _collectionRepository.GetByBrand_IDAndIsActiveToList(AppGlobal.TAStudioBrandID, true);
            json = JsonConvert.SerializeObject(listCollection);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.CollectionActive + AppGlobal.SaloneBrandID + AppGlobal.JSONExtension;
            listCollection = await _collectionRepository.GetByBrand_IDAndIsActiveToList(AppGlobal.SaloneBrandID, true);
            json = JsonConvert.SerializeObject(listCollection);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.TypeActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.Type> listType = await _typeRepository.GetByIsActiveToList(true);
            json = JsonConvert.SerializeObject(listType);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.TypeActive + AppGlobal.International + AppGlobal.JSONExtension;
            listType = _typeRepository.GetByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listType);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.TypeActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listType = _typeRepository.GetByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listType);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.BrandActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.Brand> listBrand = _brandRepository.GetByIsActiveToList(true);
            json = JsonConvert.SerializeObject(listBrand);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.BrandActive + AppGlobal.International + AppGlobal.JSONExtension;
            listBrand = _brandRepository.GetByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listBrand);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.BrandActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listBrand = _brandRepository.GetByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listBrand);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.DesignerActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.Designer> listDesigner = _designerRepository.GetByIsActiveToList(true);
            json = JsonConvert.SerializeObject(listDesigner);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.DesignerActive + AppGlobal.International + AppGlobal.JSONExtension;
            listDesigner = _designerRepository.GetByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listDesigner);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.DesignerActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listDesigner = _designerRepository.GetByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listDesigner);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.LifeStyleActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.LifeStyle> listLifeStyle = await _lifeStyleRepository.GetByIsActiveToList(true);
            json = JsonConvert.SerializeObject(listLifeStyle);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.LifeStyleActive + AppGlobal.International + AppGlobal.JSONExtension;
            listLifeStyle = _lifeStyleRepository.GetByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listLifeStyle);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.LifeStyleActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listLifeStyle = _lifeStyleRepository.GetByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listLifeStyle);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.StyleActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.Style> listStyle = await _styleRepository.GetByIsActiveToList(true);
            json = JsonConvert.SerializeObject(listStyle);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.StyleActive + AppGlobal.International + AppGlobal.JSONExtension;
            listStyle = _styleRepository.GetByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listStyle);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.StyleActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listStyle = _styleRepository.GetByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listStyle);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.ShapeActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.Shape> listShape = await _shapeRepository.GetByIsActiveToList(true);
            json = JsonConvert.SerializeObject(listShape);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.ShapeActive + AppGlobal.International + AppGlobal.JSONExtension;
            listShape = _shapeRepository.GetByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listShape);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.ShapeActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listShape = _shapeRepository.GetByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listShape);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.RoomAndUsageActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.RoomAndUsage> listRoomAndUsage = await _roomAndUsageRepository.GetByIsActiveToList(true);
            json = JsonConvert.SerializeObject(listRoomAndUsage);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.RoomAndUsageActive + AppGlobal.International + AppGlobal.JSONExtension;
            listRoomAndUsage = _roomAndUsageRepository.GetByIsActiveAndIsActiveTAUSToList(true, false);
            json = JsonConvert.SerializeObject(listRoomAndUsage);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.RoomAndUsageActive + AppGlobal.TAUS + AppGlobal.JSONExtension;
            listRoomAndUsage = _roomAndUsageRepository.GetByIsActiveAndIsActiveTAUSToList(true, true);
            json = JsonConvert.SerializeObject(listRoomAndUsage);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
            using (FileStream fs = new FileStream(physicalPathCreate, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(json);
                }
            }

            fileName = AppGlobal.ItemActive + AppGlobal.JSONExtension;
            List<TA.Data2021.Models.Item> listItem = _itemRepository.GetByIsActiveToList(true);
            json = JsonConvert.SerializeObject(listItem);
            physicalPathCreate = Path.Combine(AppGlobal.WebRootPath, subPath, fileName);
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
        [HttpGet]
        public async Task<int> AsyncUpdateItemsImageCount()
        {
            return await _itemRepository.AsyncUpdateItemsImageCount();
        }
        private void Initialization()
        {
            _itemRepository.UpdateItemsURLCode();
            _itemRepository.UpdateItemsDescription();            
            DownloadItemImagesWithImageIsNull();
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
    }
}
