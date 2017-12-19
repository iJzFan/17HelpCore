using System.Threading.Tasks;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Register;

namespace HELP.Service.UIDevService
{
    public class RegisterService : IRegisterService
    {
        public void Do(HomeModel model)
        {
            
        }

        Task IRegisterService.Do(HomeModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
