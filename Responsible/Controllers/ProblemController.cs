using HELP.GlobalFile.Global;
using HELP.GlobalFile.Global.Helper;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Problem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HELP.UI.Responsible.Controllers
{

    public class ProblemController : Controller
    {
        #region Constructor

        IHostingEnvironment _env;
        IBaseService _baseService;
        IProblemService _problemService;
        public ProblemController(IProblemService problemService, IBaseService baseService, IHostingEnvironment env)
        {
            _problemService = problemService;
            _baseService = baseService;
            _env = env;
        }

        #endregion

        #region URL: /Problem/Index/{Id}

        public async Task<IActionResult> Index(int Id = 1)
        {
            
            var tuple = await _problemService.Get(Id, 5);

            TempData["Page"] = Id;

            TempData["Count"] = tuple.count;

            return View(tuple.index);
        }

        #endregion

        #region URL: /Problem/New
        [Authorize]
        public IActionResult New()
        {
            
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> New(NewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _baseService.GetCurrentUser();

            if (model.Reward > user.Creditpoints)
            {
                ModelState.AddModelError("Reward", "* 悬赏的时间币大于现有的时间币");
                return View(model);
            }

            string path = null;
            //只要上传了图片，就要检查图片的格式和大小
            if (model.Picture != null)
            {
                var contentType = model.Picture.ContentType.ToLower();
                if (!(contentType == "image/jpeg" || contentType == "image/png" || contentType == "image/gif"))
                {
                    ModelState.AddModelError("Picture", "* 图片格式只能为jpg/png/gif");
                    return View(model);
                }
                if (model.Picture.Length > Length.ImageMaxLength)
                {
                    ModelState.AddModelError("Picture", "* 图片大小不超过1M");
                    return View(model);
                }

                //把图片存储到磁盘
                path = await FileHelper.Save(model.Picture,_env.WebRootPath);
            }


            //保存model和图片地址到数据库
            await _problemService.Save(model, user, path); 

            return RedirectToAction("Index");

        }


        private async Task<string> save(IFormFile picture)
        {
            DateTime now = DateTime.Now;
            string urlPrefix = string.Format($"{now.Year}\\{now.Month}\\{now.Day}");
            string directory = Path.Combine(_env.WebRootPath, urlPrefix);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string fileName = Path.Combine(DateTime.Now.ToString("HH_mm_ss")+"_"+Path.GetFileName(picture.FileName));
            string relativePath = Path.Combine(urlPrefix, fileName);
            using (var fs = System.IO.File.Create(Path.Combine(directory, fileName)))
            {
                await picture.CopyToAsync(fs);
                await fs.FlushAsync();
            }
            return relativePath;
        }

        #endregion

        #region URL: /Problem/Single/{id}

        public async Task<IActionResult> Single(int id)
        {
             var model = new SingleModel();
             model.Item= await _problemService.GetItem(id);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Single(int id, SingleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _problemService.SaveComment(model, id);
            return Redirect($"/Problem/{id}");
        }

        #endregion

        #region Ajax

        [Authorize]
        /// <returns>酬谢的时间币数量</returns>
        public async Task<IActionResult> Reward(int commentId)
        {
            int reward =await _problemService.Reward(commentId);
            return Json(reward);
        }


        [Authorize]
        public async Task<IActionResult> Cancel(int problemId)
        {
            await _problemService.Cancel(problemId);
            return Json(true);
        }

        #endregion


    }
}