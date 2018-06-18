using HELP.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.Controllers
{
	public class CreditController : Controller
	{
		#region Constructor

		private ICreditService _creditService;

		public CreditController(ICreditService creditService)
		{
			_creditService = creditService;
		}

		#endregion Constructor

		#region URL: /Credit/Index

		[Authorize]
		public async Task<IActionResult> Index()
		{
			var model = await _creditService.Get();
			return View(model);
		}

		#endregion URL: /Credit/Index
	}
}