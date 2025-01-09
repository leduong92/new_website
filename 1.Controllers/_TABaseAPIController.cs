using System.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TA_Web_2020_API.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TABaseAPIController : ApiController
    {
        protected static string TA_Environment = ConfigurationManager.AppSettings["TA-Environment"];
        protected static readonly Helper.Helper _helper = new Helper.Helper();
    }
}
