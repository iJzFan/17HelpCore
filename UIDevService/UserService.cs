using System.Threading.Tasks;
using HELP.Service.ServiceInterface;

namespace HELP.Service.UIDevService
{
    public class UserService:IUserService
    {

        public Task<bool> NameIsExistAsync(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
