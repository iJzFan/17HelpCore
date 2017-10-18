using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HELP.Service.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Responsible.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Problem/{id}")]
    public class ProblemController : Controller
    {
        #region Constructor


        IProblemService _problemService;
        public ProblemController(IProblemService problemService)
        {
            _problemService = problemService;
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> Get(int Id = 1)
        {
            var problem = await _problemService.GetItem(Id);
            var comments = await _problemService.GetComments(Id);

            return Json(new { problem,comments});
        }
    }
}