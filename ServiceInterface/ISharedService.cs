using HELP.Service.ViewModel.Shared;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
    public interface ISharedService
    {
        Task<NavigatorModel> Get();
    }
}
