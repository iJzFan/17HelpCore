using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using HELP.GlobalFile.Global;
using System.Threading.Tasks;

namespace HELP.Service.UIDevService
{
    public class SharedService : ISharedService
    {

        public LoginStatusModel Get()
        {
            LoginStatusModel model = new LoginStatusModel();
            if (true)
            {
                model.UserName = "叶子";
            }
            return model;
        }

        public LoginStatusModel Get(IHttpContextAccessor httpContextAccessor)
        {
            var model = new LoginStatusModel();
            if(httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CookieName.USER_ID, out string value))
            {
                model.UserName = value;
            }
            return model;
        }

        Task<LoginStatusModel> ISharedService.Get()
        {
            throw new NotImplementedException();
        }
    }
}
