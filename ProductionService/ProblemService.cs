using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Problem;
using HELP.Service.ViewModel.Problem.Single;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HELP.Service.ProductionService
{
    public class ProblemService : BaseService, IProblemService
    {
        #region Construtor
        public ProblemService(EFDbContext context, IHttpContextAccessor httpContextAccessor, IEncrypt encrypt) : base(context, httpContextAccessor, encrypt)
        {
        }
        #endregion

        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <returns>problem list</returns>
        public async Task<(IndexModel index,int count)> Get(int id,int pagesize)
        {
            var list = new List<ItemModel>();

            var problems = await _context.Problems
                .Include(x => x.Author)
                .OrderByDescending(x=>x.CreateTime).Skip((id-1)*pagesize).Take(pagesize)
                .AsNoTracking().ToListAsync();

            var count = _context.Problems.Count() / pagesize;

            foreach (var problem in problems)
            {
                var item = new ItemModel
                {
                    AuthorId = problem.Author.Id,
                    AuthorName = problem.Author.Name,
                    Body = problem.Body,
                    CreateTime = problem.CreateTime,
                    Id = problem.Id,
                    Reward = problem.Reward,
                    Title = problem.Title
                };
                list.Add(item);
            }

            var index = new IndexModel() { Items = list };

            (IndexModel index, int count) GetIndex() => (index, count);

            return GetIndex();

        }


        /// <summary>
        /// 获取评论列表 TODO:分页
        /// </summary>
        /// <param name="id">problemId</param>
        /// <returns>comments</returns>
        public async Task<CommentsModel> GetComments(int id)
        {
            var comments =await _context.Comments.Where(x=>x.ProblemId==id).Include(x => x.Author).OrderByDescending(x=>x.CreateTime).ToListAsync();

            var list = new List<ViewModel.Shared.Comment.ItemModel>();

            foreach (var comment in comments)
            {
                var item = new ViewModel.Shared.Comment.ItemModel();
                item.AuthorId = comment.Author.Id;
                item.AuthorName = comment.Author.Name;
                item.Body = comment.Body;
                item.CreateTime = comment.CreateTime;
                list.Add(item);
            }

            return new CommentsModel { List=list};
        }


        /// <summary>
        /// 获取单个问题
        /// </summary>
        /// <param name="id">problemId</param>
        /// <returns>singel problem</returns>
        public async Task<ItemModel> GetItem(int id)
        {
            var problem = await _context.Problems.Include(x=>x.Author).SingleOrDefaultAsync(x => x.Id == id);
            var model = new ItemModel
            {
                AuthorId = problem.Author.Id,
                AuthorName = problem.Author.Name,
                Body = problem.Body,
                CreateTime = problem.CreateTime,
                Id = problem.Id,
                Reward = problem.Reward,
                Title = problem.Title
            };
            return model;
        }


        /// <summary>
        /// 发布问题
        /// </summary>
        /// <param name="model"></param>
        /// <returns>porblem index</returns>
        public async Task Save(NewModel model,string attachment)
        {
            var user =await GetCurrentUser();

            var problem = new Problem
            {
                Title = model.Title,
                Body = model.Body,
                UserId = user.Id,
                //CreateTime = DateTime.Now,
                Reward = 0,
                Attachment = attachment
            };
            if (model.Reward.HasValue)
            {
                problem.Reward = model.Reward.Value;
            }

            var abc = await _context.Problems.AddAsync(problem);
            await _context.SaveChangesAsync();
            await _context.Problems.Include(x=>x.Author).ThenInclude(x=>x.CreditHistory).LastOrDefaultAsync(x => x.UserId == user.Id);
            problem.Publish();
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// 发布评论
        /// </summary>
        /// <param name="model"></param>
        /// <param name="problemId"></param>
        /// <returns></returns>
        public async Task<ViewModel.Shared.Comment.ItemModel> SaveComment(SingleModel model, int problemId)
        {
            var author = await GetCurrentUser();
            var comment = new Comment
            {
                Body=model.Body,
                //CreateTime=DateTime.Now,
                ProblemId=problemId,
                UserId=author.Id
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return new ViewModel.Shared.Comment.ItemModel
            {
                Body = comment.Body,
                AuthorId = author.Id,
                AuthorName = author.Name,
                CreateTime = comment.CreateTime
            };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<int> Reward(int commentId)
        {
            var comment = await _context.Comments.Include(x=>x.Problem).Include(x=>x.Author).SingleOrDefaultAsync(x=>x.Id==commentId);

            int currentUserId =(await GetCurrentUser()).Id;

            //不能自己酬谢自己
            if (comment.Author.Id == currentUserId)
            {
                throw new Exception(string.Format(
                    "不能自己（ID={0}）酬谢自己。酬谢的CommentId={1}",
                    currentUserId, commentId));
            }

            //不能在别人的求助上酬谢
            if (comment.Problem.Author.Id != currentUserId)
            {
                throw new Exception(string.Format(
                    "用户Id={0}试图在他不是作者的求助（Id={1}）上进行酬谢。酬谢的CommentId={2}"
                    , currentUserId, comment.Problem.Id, commentId));
            }

            comment.BeRewarded();

            await _context.SaveChangesAsync();

            return comment.Problem.Reward;
        }

    }

}
