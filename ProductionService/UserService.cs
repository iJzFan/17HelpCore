using HELP.BLL.EntityFrameworkCore;
using HELP.Service.ServiceInterface;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        #endregion

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
