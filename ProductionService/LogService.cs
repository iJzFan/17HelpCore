﻿using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Log;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HELP.Service.ProductionService
{
	public class LogService : BaseService, ILogService
	{
		#region Constructor

		public LogService(EFDbContext _context, IHttpContextAccessor _httpContextAccessor, IEncrypt _encrypt) : base(_context, _httpContextAccessor, _encrypt)
		{
		}

		#endregion Constructor

		/// <summary>
		/// 获取用户密码
		/// </summary>
		/// <param name="name">用户名</param>
		/// <returns></returns>
		public async Task<User> GetUser(string name)
		{
			var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name);
			return user;
		}

		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="model"></param>
		/// <param name="remember"></param>
		/// <returns></returns>
		public async Task On(OnModel model, bool remember)
		{
			var httpcontext = _httpContextAccessor.HttpContext;
			var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Name == model.UserName);
			var claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.SerialNumber, user.Id), new Claim(ClaimTypes.Name, user.Name), new Claim(ClaimTypes.NameIdentifier, user.AuthCode), new Claim(ClaimTypes.Role, user.Role.ToString()) }, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(claimsIdentity);
			await httpcontext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
			{
				ExpiresUtc = DateTime.UtcNow.AddHours(24),
				IsPersistent = true,
				AllowRefresh = false
			});
			//await _signInManager.PasswordSignInAsync(model.UserName, model.Password,false, lockoutOnFailure: false);

			//int? days = remember ? 1 : (int?)null;
			//SetUserCookie(user, days);
		}

		/// <summary>
		/// 注销
		/// </summary>
		/// <returns></returns>
		public async Task Off()
		{
			// if (_httpContextAccessor.HttpContext.Request.Cookies[CookieName.USER_ID] != null)
			// {
			//    _httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieName.USER_ID);
			//};
			await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}
	}
}