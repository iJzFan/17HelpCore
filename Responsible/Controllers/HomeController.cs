using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.Service.ServiceInterface;
using HELP.UI.Responsible.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.Controllers
{

    public class HomeController : Controller
    {

        public IActionResult Index()
        {


            return Redirect("/Problem/Index");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View("About");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
