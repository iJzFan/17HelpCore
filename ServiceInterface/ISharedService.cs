using HELP.Service.ViewModel.Shared;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
	public interface ISharedService
	{
		Task<NavigatorModel> Get();
	}
}