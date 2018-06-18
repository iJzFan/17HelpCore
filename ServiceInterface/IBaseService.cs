using HELP.BLL.Entity;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
	public interface IBaseService
	{
		Task<User> GetCurrentUser();
	}
}