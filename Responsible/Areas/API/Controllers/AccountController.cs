using HELP.Service.ViewModel.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users=HELP.GlobalFile.Global.Story.User;

namespace HELP.UI.Responsible.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {

        public OnModel Get()
        {
            return new OnModel {UserName = Users.yezi_UserName, Password = Users.yezi_PassWord, ImageCode = null};
        }
    }
}