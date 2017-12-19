using System.Threading.Tasks;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Log;
using Microsoft.AspNetCore.Http;

namespace HELP.Service.UIDevService
{
    public class LogService : ILogService
    {
        public string GetPassword(string name)
        {
            return "1234";
        }

        public Task Off()
        {
            throw new System.NotImplementedException();
        }

        public void On(OnModel model, IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Append("user",model.UserName);
        }

        public Task On(OnModel model, bool remember)
        {
            throw new System.NotImplementedException();
        }

        Task<string> ILogService.GetPassword(string name)
        {
            throw new System.NotImplementedException();
        }


    }
}
