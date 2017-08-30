using HELP.BLL.EntityFrameworkCore;
using HELP.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.ViewComponents
{
    public class SingleProblemViewComponent : ViewComponent
    {
        #region Constructor

        private IProblemService _problemService;
        private EFDbContext _context;
        public SingleProblemViewComponent(IProblemService problemService, EFDbContext context)
        {
            _problemService = problemService;
            _context = context;

        }
        #endregion

        #region Get single problem
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return View(await _problemService.GetItem(id));
        }
        #endregion
    }
}
