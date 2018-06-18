using HELP.Service.ViewModel.Contact;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
	public interface IContactService
	{
		Task<RecordModel> Get();

		Task Save(RecordModel model);

		Task<RecordModel> Get(string userId);
	}
}