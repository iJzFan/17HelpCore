using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        #endregion

        #region Get LoginStatus
        public async Task<IViewComponentResult> InvokeAsync()
        {
            NavigatorModel model = await _sharedService.Get();
            return View(model);
        }
        #endregion
    }
}
