using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Contact;
using HELP.UI.Responsible.WebHelp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

		#endregion Constructor

		#region URL: /Contact/Record

		[Authorize]
		public async Task<IActionResult> Record(string returnUrl = null)
		{
			TempData["ReturnUrl"] = returnUrl;
			var model = await _contactService.Get();
			return View(model);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Record(RecordModel model, string returnUrl = null)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await _contactService.Save(model);

			return ReturnUrlHelper.ReturnUrl(returnUrl);
		}

		#endregion URL: /Contact/Record

		#region URL: /Contact/Show

		public async Task<IActionResult> Show(string Id)
		{
			var model = new RecordModel();

			model = await _contactService.Get(Id);

			return PartialView(model);
		}

		#endregion URL: /Contact/Show
	}
}