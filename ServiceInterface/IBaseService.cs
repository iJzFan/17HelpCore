using HELP.BLL.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
    public interface IBaseService
    {
        Task<User> GetCurrentUser();

    }
}
