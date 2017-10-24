using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HELP.Service.ServiceInterface;
using Microsoft.AspNetCore.Authorization;

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

        #endregion

        #region URL: /Credit/Index

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var model =await _creditService.Get();
            return View(model);
        }

        #endregion

    }
}