using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global;
using HELP.GlobalFile.Global.Encryption;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HELP.Service.ProductionService
{
    public class BaseService
    {
        #region Constructor
        protected EFDbContext _context;
        protected IHttpContextAccessor _httpContextAccessor;
        protected IEncrypt _encrypt;

        public BaseService(EFDbContext context, IHttpContextAccessor httpContextAccessor, IEncrypt encrypt)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _encrypt = encrypt;
        }
        #endregion

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns>如果用户没有登陆，返回null</returns>
        protected async Task<User> GetCurrentUser()
        {
            var Context = _httpContextAccessor.HttpContext;
            string key = CookieName.USER_ID;

            var userIdentity = Context.Request.Cookies[key];
            if (userIdentity == null)
            {
                return null;
            }

            try
            {
                int userId = Convert.ToInt32(userIdentity);
                string encryptInfo = Context.Request.Cookies[CookieName.AUTH_CODE];
                if (string.IsNullOrEmpty(encryptInfo))
                {
                    string message = string.Format("cookie for {0}={1} has no encryption.", key, userId);
                    throw new Exception(message);
                }
                var user = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    string message = string.Format("cookie for user：{1} has doesn't exist.", userId);
                    throw new Exception(message);
                }
                if (_encrypt.Encrypt(user.AuthCode) != encryptInfo)
                {
                    throw new Exception(string.Format("cookie for {0}={1} has wrong encryption.", key, userId));
                }
                return user;
            }
            catch (Exception e)
            {
                //TODO: workaround for the e.Message throw out exceptions.
                while (e.InnerException != null) e = e.InnerException;

                //TODO: where? how?
                //new LogService().Off();

                //TODO: probably need more information later.
                // var log = log4net.LogManager.GetLogger("KnownError");
                //log.Error(e.Message);

                return null;

            }
        }

        //protected Role getCurrentUserRole()
        //{
        //    HttpCookie userIdentity = HttpContext.Current.Request.Cookies[CookieName.USER_ID];
        //    if (userIdentity == null)
        //    {
        //        return Role.Visitor;
        //    }
        //    return (Role)Enum.Parse(typeof(Role), userIdentity.Values[CookieName.ROLE]);
        //}

            /// <summary>
            /// 设置Cookie
            /// </summary>
            /// <param name="user"></param>
            /// <param name="days"></param>
        protected void SetUserCookie(User user, /*Role role,*/ int? days = null)
        {

            var opt = new CookieOptions();

            if (days.HasValue)
            {
                opt.Expires = DateTime.Now.AddDays(days.Value);
            }

            _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieName.USER_ID, user.Id.ToString(), opt);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieName.AUTH_CODE, _encrypt.Encrypt(user.AuthCode), opt);

        }
    }
}
