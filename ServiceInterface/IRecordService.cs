using HELP.Service.ViewModel.Contact;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
    public interface IContactService
    {
        Task<RecordModel> Get();
        Task Save(RecordModel model);
        Task<RecordModel> Get(int userId);
    }
}
