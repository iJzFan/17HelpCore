using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
	public interface IUserService
	{
		Task<bool> NameIsExistAsync(string name);
	}
}