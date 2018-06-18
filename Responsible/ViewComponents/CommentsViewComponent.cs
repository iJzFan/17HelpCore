using HELP.BLL.EntityFrameworkCore;
using HELP.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.ViewComponents
{
	public class CommentsViewComponent : ViewComponent
	{
		#region Constructor

		private IProblemService _problemService;
		private EFDbContext _context;

		public CommentsViewComponent(IProblemService problemService, EFDbContext context)
		{
			_problemService = problemService;
			_context = context;
		}

		#endregion Constructor

		#region Get Comment

		public async Task<IViewComponentResult> InvokeAsync(int id)
		{
			return View(await _problemService.GetComments(id));
		}

		#endregion Get Comment
	}
}