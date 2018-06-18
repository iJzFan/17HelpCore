using HELP.BLL.EntityFrameworkCore;
using HELP.Service.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HELP.Service.ProductionService
{
	public class UserService : IUserService
	{
		#region Constructor

		private EFDbContext _context;

		public UserService(EFDbContext context)
		{
			_context = context;
		}

		#endregion Constructor

		/// <summary>
		/// 检查用户名是否存在
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public async Task<bool> NameIsExistAsync(string name)
		{
			var userName = _context.Users.Where(c => c.Name == name).AsNoTracking().SingleOrDefaultAsync();
			return await userName != null;
		}
	}
}