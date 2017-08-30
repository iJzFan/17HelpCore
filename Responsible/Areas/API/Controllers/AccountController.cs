using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HELP.Service.ViewModel.Log;
using Users=HELP.GlobalFile.Global.Story.User;

namespace Responsible.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        public OnModel GET()
        {
            return new OnModel {UserName= Users.yezi_UserName,Password=Users.yezi_PassWord,ImageCode=null };
        }
    }
}