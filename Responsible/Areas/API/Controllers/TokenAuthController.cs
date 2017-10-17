using System;
//using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HELP.Service.ViewModel.Log;
using HELP.Service.ServiceInterface;
using HELP.GlobalFile.Global.Encryption;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Helper;
using HELP.BLL.Entity;

namespace HELP.UI.Responsible.Areas.API.Controllers
{

    [Route("api/[controller]")]
    public class TokenAuthController : Controller
    {
        #region Constructor

        private IRegisterService _registerService;
        private IUserService _userService;
        private ILogService _logService;
        private IEncrypt _encrypt;
        private EFDbContext _context;

        public TokenAuthController(IRegisterService registerService,
            IUserService userService,
            ILogService logService, IEncrypt encrypt, EFDbContext context)
        {
            _registerService = registerService;
            _userService = userService;
            _logService = logService;
            _encrypt = encrypt;
            _context = context;
        }

        #endregion
/// <summary>
/// Request UserName and Password
/// </summary>
/// <param name="model"></param>
/// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(OnModel model)
        {
            //User existUser = UserStorage.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            var existUser = await _context.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Name == model.UserName);
            if (existUser != null)
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
                    Msg = "Username or password is invalid"
                });
            }
        }

        private string GenerateToken(User user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Name, "TokenAuth"),
                new[] { new Claim("ID", user.Id.ToString())}
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
/// <summary>
/// Need jwt token to access
/// </summary>
/// <returns></returns>
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == claimsIdentity.Name);
            return Json(new RequestResult
            {
                State = RequestState.Success,
                Data = new { UserName = claimsIdentity.Name, Credit = user.Creditpoints }
            });
        }
    }
}

