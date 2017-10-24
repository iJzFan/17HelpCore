using HELP.GlobalFile.Global.Encryption;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Log;
using HELP.Service.ViewModel.Shared;
using HELP.UI.Responsible.WebHelp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.Controllers
{
    public class LogController : Controller
    {

        #region Constructor

        private IRegisterService _registerService;
        private IUserService _userService;
        private ILogService _logService;
        private IEncrypt _encrypt;
        private IDistributedCache _distributedCache;
        public LogController(IRegisterService registerService,
            IUserService userService,
            ILogService logService, IEncrypt encrypt, IDistributedCache distributedCache)
        {
            _registerService = registerService;
            _userService = userService;
            _logService = logService;
            _encrypt = encrypt;
            _distributedCache = distributedCache;
        }

        #endregion

        #region URL: /Log/On

        public IActionResult On(string returnUrl = null)
        {
            //await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View(new OnModel());
        }

        [HttpPost]
        public async Task<IActionResult> On(OnModel model, bool remember, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var sessionImageCode = _distributedCache.GetString(ImageCodeHelper.SESSION_IMAGE_CODE);
            var sessionImageCode = HttpContext.Session.GetString(ImageCodeHelper.SESSION_IMAGE_CODE);
            model.ImageCode = ImageCodeHelper.CheckResult(model.ImageCode, sessionImageCode);
            if (model.ImageCode.ImageCodeError != ImageCodeError.NoError)
            {
                return View(model);
            }

            var existUser = await _logService.GetUser(model.UserName);

            if (existUser != null && existUser.Password == _encrypt.Encrypt(model.Password))
            {
                await _logService.On(model, remember);
                return ReturnUrlHelper.ReturnUrl(returnUrl);
            }

            TempData["ModelState"] = "* 用户名或密码错误";
            return View(model);
        }
        #endregion

        #region URL:/Log/Off
        public async Task<IActionResult> Off()
        {
            await _logService.Off();

            return Redirect("~/Log/On");
        }

        #endregion

        #region URL: /Log/AccessDenied

        public IActionResult AccessDenied()
        {

            return View();
        }
        #endregion
    }
}
