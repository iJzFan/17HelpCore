using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HELP.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using HELP.Service.ViewModel.Contact;
using HELP.UI.Responsible.WebHelp;

namespace HELP.UI.Responsible.Controllers
{
    public class ContactController : Controller
    {
        #region Constructor
        private IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        #endregion

        #region URL: /Contact/Record

        [Authorize]
        public async Task<IActionResult> Record(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            var model =await _contactService.Get();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Record(RecordModel model,string returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _contactService.Save(model);

            return ReturnUrlHelper.ReturnUrl(returnUrl);
        }

        #endregion

        #region URL: /Contact/Show

        public async Task<IActionResult> Show(string Id)
        {
            var model = new RecordModel();

                model = await _contactService.Get(Id);

            return PartialView(model);
        }

        #endregion
    }
}