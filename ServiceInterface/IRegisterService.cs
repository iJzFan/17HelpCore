using HELP.Service.ViewModel.Register;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
    public interface IRegisterService
    {
        Task Do(HomeModel model);
    }
}