using HELP.Service.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/{id}")]
    public class ProblemsController:Controller
    {
        #region Constructor


        IProblemService _problemService;
        public ProblemsController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> Get(int Id = 1)
        {
            var tuple = await _problemService.Get(Id, 5);
            return Json(tuple);
        }

    }
}
