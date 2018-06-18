using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Log;
using HELP.Service.ViewModel.Register;
using HELP.Service.ViewModel.Shared;
using HELP.UI.Responsible.WebHelp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.Controllers
{
	public class RegisterController : Controller
	{
		#region Constructor

		private IRegisterService _registerService;
		private IUserService _userService;

		private ILogService _logService;

		public RegisterController(IRegisterService registerService,
			IUserService userService, ILogService logService)
		{
			_registerService = registerService;
			_userService = userService;

			_logService = logService;
		}

		#endregion Constructor

		#region URL: /Register/Home

		public IActionResult Home(string returnUrl = null)
		{
			TempData["ReturnUrl"] = returnUrl;
			return View(new HomeModel());
		}

		[HttpPost]
		public async Task<IActionResult> Home(HomeModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var sessionImageCode = HttpContext.Session.GetString(ImageCodeHelper.SESSION_IMAGE_CODE);
			model.ImageCode = ImageCodeHelper.CheckResult(model.ImageCode, sessionImageCode);

			if (model.ImageCode.ImageCodeError != ImageCodeError.NoError)
			{
				return View(model);
			}

			if (await _userService.NameIsExistAsync(model.UserName))
			{
				ModelState.AddModelError("UserName", "* 用户名已经被使用");
				return View(model);
			}

			await _registerService.Do(model);

			await _logService.On(new OnModel()
			{
				UserName = model.UserName,
				Password = model.Password
			}, true);

			return ReturnUrlHelper.ReturnUrl(returnUrl);
		}

		#endregion URL: /Register/Home

		#region Ajax

		#region URL：/Register/UserNameIsExist

		public async Task<IActionResult> UserNameIsExist(string name)
		{
			return Json(await _userService.NameIsExistAsync(name));
		}

		#endregion URL：/Register/UserNameIsExist

		#endregion Ajax
	}
}