using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.WebHelp
{
    public class ReturnUrlHelper
    {
        public static IActionResult ReturnUrl(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return new RedirectResult(returnUrl);
            }

            return new RedirectResult("/Problem/Index");
        }
    }
}
