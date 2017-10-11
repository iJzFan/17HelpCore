using HELP.Service.ServiceInterface;
using System;
using HELP.Service.ViewModel.Register;
using System.Threading.Tasks;
using HELP.BLL.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using HELP.BLL.Entity;
using HELP.GlobalFile.Global.Encryption;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace HELP.Service.ProductionService
{
    public class RegisterService : BaseService, IRegisterService
    {
        #region Constructor
        public RegisterService(EFDbContext _context, IHttpContextAccessor _httpContextAccessor, IEncrypt _encrypt, UserManager<User> _userManager, SignInManager<User> _signInManager) : base(_context, _httpContextAccessor, _encrypt, _userManager, _signInManager)
        {

        }
        #endregion


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Do(HomeModel model)
        {
            var password = _encrypt.Encrypt(model.Password);
            var user = new User()
            {
                Name = model.UserName,
                Password = password,
                contact = new Contact()
            };
            user.SetAuthCode();
            user.Register();
            _context.Users.Add(user);
            _context.Contacts.Add(user.contact);
            _context.Credit.Add(user.CreditHistory.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
    }
}
