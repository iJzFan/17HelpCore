using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global;
using HELP.GlobalFile.Global.Helper;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Problem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Responsible.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Problem")]
    public class ProblemController : Controller
    {
        #region Constructor


        IProblemService _problemService;
        EFDbContext _context;
        IHostingEnvironment _env;
        public ProblemController(IProblemService problemService, EFDbContext context,IHostingEnvironment env)
        {
            _problemService = problemService;
            _context = context;
            _env = env;
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> Get(int Id = 1)
        {
            var problem = await _problemService.GetItem(Id);
            var comments = await _problemService.GetComments(Id);

            return Json(new { problem,comments});
        }

        [HttpPost]
        [Authorize("Bearer")]
        public async Task<IActionResult> Post(NewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState);
            }

            var claimsIdentity = User.Identity as ClaimsIdentity;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == claimsIdentity.Name);

            if (model.Reward > user.Creditpoints)
            {
                ModelState.AddModelError("Reward", "* 悬赏的时间币大于现有的时间币");
                return Json(ModelState);
            }

            string path = null;
            //只要上传了图片，就要检查图片的格式和大小
            if (model.Picture != null)
            {
                var contentType = model.Picture.ContentType.ToLower();
                if (!(contentType == "image/jpeg" || contentType == "image/png" || contentType == "image/gif"))
                {
                    ModelState.AddModelError("Picture", "* 图片格式只能为jpg/png/gif");
                    return Json(ModelState);
                }
                if (model.Picture.Length > Length.ImageMaxLength)
                {
                    ModelState.AddModelError("Picture", "* 图片大小不超过1M");
                    return Json(ModelState);
                }

                //把图片存储到磁盘
                path = await FileHelper.Save(model.Picture, _env.WebRootPath);
            }

            //保存model和图片地址到数据库
            await _problemService.Save(model, user, path);

            return Ok();
        }
    }
}