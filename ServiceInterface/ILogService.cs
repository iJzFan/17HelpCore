using HELP.Service.ViewModel.Log;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
    public interface ILogService
    {
       Task On(OnModel model,bool remember);

        Task<string> GetPassword(string name);

        Task Off();
    }
}
