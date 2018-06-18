using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.ViewComponents
{
	public class NavigatorViewComponent : ViewComponent
	{
		#region Constructor

		private ISharedService _sharedService;

		public NavigatorViewComponent(ISharedService sharedService)
		{
			_sharedService = sharedService;
		}

		#endregion Constructor

		#region Get LoginStatus

		public async Task<IViewComponentResult> InvokeAsync()
		{
			NavigatorModel model = await _sharedService.Get();
			return View(model);
		}

		#endregion Get LoginStatus
	}
}