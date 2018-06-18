using HELP.UI.Responsible.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
			ViewData["Message"] = "ASP.NET Core 演示网站";

			return View("About");
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "唯一联系QQ:337845818.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}