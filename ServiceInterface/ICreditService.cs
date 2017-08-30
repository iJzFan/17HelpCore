using HELP.Service.ViewModel.Credit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
    public interface ICreditService
    {
        Task<IndexModel> Get();
    }
}
