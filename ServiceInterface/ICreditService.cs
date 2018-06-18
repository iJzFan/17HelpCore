using HELP.Service.ViewModel.Credit;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
	public interface ICreditService
	{
		Task<IndexModel> Get();
	}
}