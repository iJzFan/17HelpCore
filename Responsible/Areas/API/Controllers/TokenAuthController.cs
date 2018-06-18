using HELP.BLL.Entity;
using HELP.GlobalFile.Global.Encryption;
using HELP.GlobalFile.Global.Helper;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Log;
using HELP.Service.ViewModel.Shared;
using HELP.UI.Responsible.WebHelp;
using Microsoft.AspNetCore.Http;

//using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.Areas.API.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	public class TokenAuthController : Controller
	{
		#region Constructor

		private ILogService _logService;
		private IEncrypt _encrypt;
		private IHttpContextAccessor _httpContextAccessor;

		public TokenAuthController(IHttpContextAccessor httpContextAccessor,
			ILogService logService, IEncrypt encrypt)
		{
			_logService = logService;
			_encrypt = encrypt;
			_httpContextAccessor = httpContextAccessor;
		}

		#endregion Constructor

		[HttpPut]
		public async Task<IActionResult> Put(OnModel model)
		{
			if (!ModelState.IsValid)
			{
				return Json(ModelState.ValidationState);
			}

			var sessionImageCode = HttpContext.Session.GetString(ImageCodeHelper.SESSION_IMAGE_CODE);
			model.ImageCode = ImageCodeHelper.CheckResult(model.ImageCode, sessionImageCode);
			if (model.ImageCode.ImageCodeError != ImageCodeError.NoError)
			{
				return Json(new RequestResult
				{
					State = RequestState.Failed,
					Msg = "验证码错误!"
				});
			}

			var existUser = await _logService.GetUser(model.UserName);

			if ((existUser != null) && (existUser.Password == _encrypt.Encrypt(model.Password)))
			{
				var requestAt = DateTime.Now;
				var expiresIn = requestAt + TokenAuthOption.ExpiresSpan;
				var token = GenerateToken(existUser, expiresIn);

				return Json(new RequestResult
				{
					State = RequestState.Success,
					Data = new
					{
						requertAt = requestAt,
						expiresIn = TokenAuthOption.ExpiresSpan.TotalSeconds,
						tokeyType = TokenAuthOption.TokenType,
						accessToken = token
					}
				});
			}
			else
			{
				return Json(new RequestResult
				{
					State = RequestState.Failed,
					Msg = "用户名或密码错误!"
				});
			}
		}

		#region Method GenerateToken

		private string GenerateToken(User user, DateTime expires)
		{
			var handler = new JwtSecurityTokenHandler();

			ClaimsIdentity identity = new ClaimsIdentity(
				new GenericIdentity(user.Name, "TokenAuth"),
				new[] { new Claim("ID", user.Id.ToString()) }
			);

			var securityToken = handler.CreateToken(new SecurityTokenDescriptor
			{
				Issuer = TokenAuthOption.Issuer,
				Audience = TokenAuthOption.Audience,
				SigningCredentials = TokenAuthOption.SigningCredentials,
				Subject = identity,
				Expires = expires
			});
			return handler.WriteToken(securityToken);
		}

		#endregion Method GenerateToken

		[HttpGet]
		public IActionResult Get()
		{
			var imageCode = _httpContextAccessor.HttpContext.Request.Host + @"/Shared/GetImageCode";

			return Json(new { imageCode });
		}
	}
}