using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Shared;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HELP.Service.ProductionService
{
	public class SharedService : BaseService, ISharedService
	{
		#region Constructor

		public SharedService(EFDbContext _context, IHttpContextAccessor _httpContextAccessor, IEncrypt _encrypt) : base(_context, _httpContextAccessor, _encrypt)
		{
		}

		#endregion Constructor

		/// <summary>
		/// 获取导航栏信息
		/// </summary>
		/// <returns></returns>
		public async Task<NavigatorModel> Get()
		{
			var user = await GetCurrentUser();

			if (user != null)
			{
				return new NavigatorModel { UserName = user.Name, Credit = user.Creditpoints };
			}

			return new NavigatorModel { UserName = null };
		}
	}
}