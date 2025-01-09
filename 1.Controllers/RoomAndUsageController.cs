using BL.CustomExceptions;
using DAL.ViewModels;
using System;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using TA_Web_2020_API.Helper;
using BL.DTO;
using BL.BUServices;
using System.Web.Script.Serialization;
using TA.Data2021.Repositories;
using System.Collections.Generic;
using TA.Helpers2021;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using TA_Web_2020_API.ViewModel;
using StyleViewModel = TA_Web_2020_API.ViewModel.StyleViewModel;

namespace TA_Web_2020_API.Controllers
{
    [RoutePrefix("api/RoomAndUsage")]
    public class RoomAndUsageController : TABaseAPIController
    {
        private readonly IRoomAndUsageRepository _roomAndUsageRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly ICollectionRepository _collectionRepository;
        private readonly ILifeStyleRepository _lifeStyleRepository;
        private readonly IShapeRepository _shapeRepository;
        private readonly IStyleRepository _styleRepository;
        public RoomAndUsageController(IRoomAndUsageRepository roomAndUsageRepository, ITypeRepository typeRepository, ICollectionRepository collectionRepository, ILifeStyleRepository lifeStyleRepository, IShapeRepository shapeRepository, IStyleRepository styleRepository) : base()
        {
            _roomAndUsageRepository = roomAndUsageRepository;
            _typeRepository = typeRepository;
            _collectionRepository = collectionRepository;
            _lifeStyleRepository = lifeStyleRepository;
            _shapeRepository = shapeRepository;
            _styleRepository = styleRepository;
        }
        [HttpGet]
        public IHttpActionResult GetByID(string ID)
        {
            TA.Data2021.Models.RoomAndUsage item = _roomAndUsageRepository.GetByID(ID);
            return new GenerateResponeHelper<TA.Data2021.Models.RoomAndUsage>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByURLCode(string URLCode)
        {
            TA.Data2021.Models.RoomAndUsage item = _roomAndUsageRepository.GetByURLCode(URLCode);
            return new GenerateResponeHelper<TA.Data2021.Models.RoomAndUsage>(item, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public IHttpActionResult GetByIsActiveAndRegionToList(bool isActive, string region)
        {
            List<TA.Data2021.Models.RoomAndUsage> list = _roomAndUsageRepository.GetByIsActiveAndIsActiveTAUSToList(isActive, AppGlobal.InitializationIsActiveTAUS(region));
            return new GenerateResponeHelper<List<TA.Data2021.Models.RoomAndUsage>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetByIsActiveToList(bool isActive)
        {
            List<TA.Data2021.Models.RoomAndUsage> list = await _roomAndUsageRepository.GetByIsActiveToList(isActive);
            return new GenerateResponeHelper<List<TA.Data2021.Models.RoomAndUsage>>(list, true, Request, HttpStatusCode.OK);
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetSideBarFilter()
        {
            List<TA.Data2021.Models.RoomAndUsage> listRoomAndUsage = await _roomAndUsageRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.Type> listType = await _typeRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.Collection> listCollection = await _collectionRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.LifeStyle> listLifeStyle = await _lifeStyleRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.Shape> listShape = await _shapeRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.Style> listStyle = await _styleRepository.GetByIsActiveToList(true);

            StringBuilder result = new StringBuilder();
            result.Append(@"{");

            result.Append(@"'Rooms':{");
            result.Append(@"'Room':[");

            for (int i = 0; i < listRoomAndUsage.Count; i++)
            {
                TA.Data2021.Models.RoomAndUsage room = listRoomAndUsage[i];
                result.Append(@"{");
                result.Append(@"'ID':'" + room.ID.ToString() + @"',");
                result.Append(@"'Name':'" + room.DisplayName + @"',");
                result.Append(@"'TitleIsImage':'0',");
                result.Append(@"'Title':'" + room.DisplayName + @"',");
                result.Append(@"'Type':[");
                for (int j = 0; j < listType.Count; j++)
                {
                    TA.Data2021.Models.Type type = listType[j];
                    if (type.RoomAndUsage_ID == room.ID)
                    {
                        result.Append(@"{");
                        result.Append(@"'ID':'" + type.ID.ToString() + @"',");
                        result.Append(@"'Name':'" + type.DisplayName + @"',");
                        result.Append(@"'TitleIsImage':'0',");
                        result.Append(@"'Title':'" + type.DisplayName + @"',");
                        if (j == listType.Count - 1)
                        {
                            result.Append(@"}");
                        }
                        else
                        {
                            result.Append(@"},");
                        }
                    }
                }
                result.Append(@"]");
                if (i == listRoomAndUsage.Count - 1)
                {
                    result.Append(@"}");
                }
                else
                {
                    result.Append(@"},");
                }
            }

            result.Append(@"]");
            result.Append(@"},");

            result.Append(@"'Collections':{");
            result.Append(@"'Collection':[");
            for (int i = 0; i < listCollection.Count; i++)
            {
                TA.Data2021.Models.Collection collection = listCollection[i];
                result.Append(@"{");
                result.Append(@"'ID':'" + collection.ID.ToString() + @"',");
                result.Append(@"'Name':'" + collection.DisplayName + @"',");
                result.Append(@"'TitleIsImage':'0',");
                result.Append(@"'Title':'" + collection.DisplayName + @"',");
                if (i == listCollection.Count - 1)
                {
                    result.Append(@"}");
                }
                else
                {
                    result.Append(@"},");
                }
            }
            result.Append(@"]");
            result.Append(@"},");

            result.Append(@"'LifeStyles':{");
            result.Append(@"'LifeStyle':[");
            for (int i = 0; i < listLifeStyle.Count; i++)
            {
                TA.Data2021.Models.LifeStyle lifeStyle = listLifeStyle[i];
                result.Append(@"{");
                result.Append(@"'ID':'" + lifeStyle.ID.ToString() + @"',");
                result.Append(@"'Name':'" + lifeStyle.DisplayName + @"',");
                result.Append(@"'TitleIsImage':'0',");
                result.Append(@"'Title':'" + lifeStyle.DisplayName + @"',");
                if (i == listLifeStyle.Count - 1)
                {
                    result.Append(@"}");
                }
                else
                {
                    result.Append(@"},");
                }
            }
            result.Append(@"]");
            result.Append(@"},");

            result.Append(@"'Styles':{");
            result.Append(@"'Style':[");
            for (int i = 0; i < listStyle.Count; i++)
            {
                TA.Data2021.Models.Style style = listStyle[i];
                result.Append(@"{");
                result.Append(@"'ID':'" + style.ID.ToString() + @"',");
                result.Append(@"'LifeStyle_ID':'" + style.LifeStyle_ID.ToString() + @"',");
                result.Append(@"'Name':'" + style.DisplayName + @"',");
                result.Append(@"'TitleIsImage':'0',");
                result.Append(@"'Title':'" + style.DisplayName + @"',");
                if (i == listStyle.Count - 1)
                {
                    result.Append(@"}");
                }
                else
                {
                    result.Append(@"},");
                }
            }
            result.Append(@"]");
            result.Append(@"},");            

            result.Append(@"'Shapes':{");
            result.Append(@"'Shape':[");
            for (int i = 0; i < listShape.Count; i++)
            {
                TA.Data2021.Models.Shape shape = listShape[i];
                result.Append(@"{");
                result.Append(@"'ID':'" + shape.ID.ToString() + @"',");
                result.Append(@"'Name':'" + shape.DisplayName + @"',");
                result.Append(@"'TitleIsImage':'0',");
                result.Append(@"'Title':'" + shape.DisplayName + @"',");
                if (i == listShape.Count - 1)
                {
                    result.Append(@"}");
                }
                else
                {
                    result.Append(@"},");
                }
            }
            result.Append(@"]");
            result.Append(@"},");

            result.Append(@"'Extending':{");
            result.Append(@"'Value':'false',");
            result.Append(@"}");

            result.Append(@"}");

            string resultString = result.ToString();
            resultString = resultString.Replace(@"\r\n\", @"");
            JObject json = JObject.Parse(resultString);            
            return new GenerateResponeHelper<JObject>(json, true, Request, HttpStatusCode.OK);
        }

        [HttpGet]
        public IHttpActionResult GetSideBarFilterViewModelByJSONURL()
        {
            SideBarFilterViewModel result = new SideBarFilterViewModel();
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("http://api2.theodorealexander.com/download/SideBarFilter.json");
                result = JsonConvert.DeserializeObject<SideBarFilterViewModel>(json);
            }
            return new GenerateResponeHelper<SideBarFilterViewModel>(result, true, Request, HttpStatusCode.OK);            
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetSideBarFilterViewModel()
        {
            SideBarFilterViewModel result = new SideBarFilterViewModel();
            List<TA.Data2021.Models.RoomAndUsage> listRoomAndUsage = await _roomAndUsageRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.Type> listType = await _typeRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.Collection> listCollection = await _collectionRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.LifeStyle> listLifeStyle = await _lifeStyleRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.Shape> listShape = await _shapeRepository.GetByIsActiveToList(true);
            List<TA.Data2021.Models.Style> listStyle = await _styleRepository.GetByIsActiveToList(true);

            result.Extending = new ExtendingViewModel();
            result.Extending.Value = false;

            result.Rooms = new RoomsViewModel();
            result.Rooms.Room = new List<RoomAndUsageViewModel>();
            foreach (TA.Data2021.Models.RoomAndUsage room in listRoomAndUsage)
            {
                RoomAndUsageViewModel roomViewModel = new RoomAndUsageViewModel();
                roomViewModel.ID = room.ID;
                roomViewModel.Name = room.Name;
                roomViewModel.Title = room.DisplayName;
                roomViewModel.TitleIsImage = 0;
                roomViewModel.URLCode = room.URLCode;
                roomViewModel.Type = new List<ViewModel.TypeViewModel>();
                foreach (TA.Data2021.Models.Type type in listType)
                {
                    if (type.RoomAndUsage_ID == roomViewModel.ID)
                    {
                        ViewModel.TypeViewModel typeViewModel = new ViewModel.TypeViewModel();
                        typeViewModel.ID = type.ID;
                        typeViewModel.Name = type.DisplayName;
                        typeViewModel.Title = type.DisplayName;
                        typeViewModel.TitleIsImage = 0;
                        typeViewModel.URLCode = type.URLCode;
                        roomViewModel.Type.Add(typeViewModel);
                    }
                }
                result.Rooms.Room.Add(roomViewModel);
            }

            result.Collections = new CollectionsViewModel();
            result.Collections.Collection = new List<CollectionViewModel>();
            foreach (TA.Data2021.Models.Collection collection in listCollection)
            {
                CollectionViewModel collectionViewModel = new CollectionViewModel();
                collectionViewModel.ID = collection.ID;
                collectionViewModel.Name = collection.Name;
                collectionViewModel.Title = collection.DisplayName;
                collectionViewModel.TitleIsImage = 0;
                collectionViewModel.URLCode = collection.URLCode;
                result.Collections.Collection.Add(collectionViewModel);
            }

            result.LifeStyles = new LifeStylesViewModel();
            result.LifeStyles.LifeStyle = new List<LifeStyleViewModel>();
            foreach (TA.Data2021.Models.LifeStyle lifeStyle in listLifeStyle)
            {
                LifeStyleViewModel lifeStyleViewModel = new LifeStyleViewModel();
                lifeStyleViewModel.ID = lifeStyle.ID;
                lifeStyleViewModel.Name = lifeStyle.Name;
                lifeStyleViewModel.Title = lifeStyle.DisplayName;
                lifeStyleViewModel.TitleIsImage = 0;
                lifeStyleViewModel.URLCode = lifeStyle.URLCode;
                result.LifeStyles.LifeStyle.Add(lifeStyleViewModel);
            }

            result.Styles = new StylesViewModel();
            result.Styles.Style = new List<StyleViewModel>();
            foreach (TA.Data2021.Models.Style style in listStyle)
            {
                StyleViewModel styleViewModel = new StyleViewModel();
                styleViewModel.ID = style.ID;
                styleViewModel.Name = style.Name;
                styleViewModel.Title = style.DisplayName;
                styleViewModel.TitleIsImage = 0;
                styleViewModel.URLCode = style.URLCode;
                result.Styles.Style.Add(styleViewModel);
            }

            result.Shapes = new ShapesViewModel();
            result.Shapes.Shape = new List<ViewModel.ShapeViewModel>();
            foreach (TA.Data2021.Models.Shape shape in listShape)
            {
                ViewModel.ShapeViewModel shapeViewModel = new ViewModel.ShapeViewModel();
                shapeViewModel.ID = shape.ID;
                shapeViewModel.Name = shape.Name;
                shapeViewModel.Title = shape.DisplayName;
                shapeViewModel.TitleIsImage = 0;
                shapeViewModel.URLCode = shape.URLCode;
                result.Shapes.Shape.Add(shapeViewModel);
            }

            return new GenerateResponeHelper<SideBarFilterViewModel>(result, true, Request, HttpStatusCode.OK);
        }

    }
}
