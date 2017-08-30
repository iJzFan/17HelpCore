using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Shared;
using System;
using Microsoft.AspNetCore.Http;
using HELP.GlobalFile.Global;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using System.Threading.Tasks;
using HELP.BLL.Entity;

namespace HELP.Service.ProductionService
{
    public class SharedService : BaseService, ISharedService
    {

        #region Constructor
        public SharedService(EFDbContext context, IHttpContextAccessor httpContextAccessor, IEncrypt encrypt) : base(context, httpContextAccessor, encrypt)
        {
        }
        #endregion


        /// <summary>
        /// 获取导航栏信息
        /// </summary>
        /// <returns></returns>
        public async Task<NavigatorModel> Get()
        {
            var user = await GetCurrentUser();

            if (user != null)
            {
                return new NavigatorModel { UserName = user.Name, Credit = user.Credit };
            }

            return new NavigatorModel { UserName = null };
        }

    }
}
