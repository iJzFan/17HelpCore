using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Register;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace HELP.Service.ProductionService
{
	public class RegisterService : BaseService, IRegisterService
	{
		#region Constructor

		public RegisterService(EFDbContext _context, IHttpContextAccessor _httpContextAccessor, IEncrypt _encrypt) : base(_context, _httpContextAccessor, _encrypt)
		{
		}

		#endregion Constructor

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
				Role = GlobalFile.Global.Role.system,
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