using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Shared;
using HELP.UI.Responsible.WebHelp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HELP.UI.Responsible.Controllers
{
    public class SharedController : Controller
    {
        #region Constructor

        private ISharedService _sharedService;
        private IHttpContextAccessor _httpContextAccessor;
        private IDistributedCache _distributedCache;
        public SharedController(ISharedService sharedService,IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache)
        {
            _sharedService = sharedService;
            _httpContextAccessor = httpContextAccessor;
            _distributedCache = distributedCache;
        }

        #endregion

        #region _ImageCode

        public IActionResult GetImageCode()
        {
            string code = ImageCodeHelper.CreateValidateCode(4);
            //_distributedCache.SetString(ImageCodeHelper.SESSION_IMAGE_CODE, code);
            HttpContext.Session.SetString(ImageCodeHelper.SESSION_IMAGE_CODE, code);
            byte[] bytes = ImageCodeHelper.CreateValidateGraphic(code);

            return File(bytes, @"image/jpeg");
        }

        #endregion



    }
}

