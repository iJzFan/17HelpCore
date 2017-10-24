using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using HELP.Service.ServiceInterface;
using HELP.Service.ViewModel.Problem;
using HELP.Service.ViewModel.Problem.Single;
using HELP.Service.ViewModel.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        public ProblemService(EFDbContext _context, IHttpContextAccessor _httpContextAccessor, IEncrypt _encrypt) : base(_context, _httpContextAccessor, _encrypt)
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

            var count = (int)Math.Ceiling(_context.Problems.Count() / (double)pagesize);

            foreach (var problem in problems)
            {
                var commentcount = await _context.Comments.CountAsync(x => x.ProblemId == problem.Id);
                var bestkind = await _context.Users.SingleOrDefaultAsync(x => x.Id == problem.RewardBestId.ToString());
                var author = new UserModel();
                author.Id = problem.Author.Id;
                author.Name = problem.Author.Name;
                var bestKindHearted = new UserModel();
                if (bestkind != null)
                {
                    bestKindHearted.Id = bestkind.Id;
                    bestKindHearted.Name = bestkind.Name;
                }
                var item = new ItemModel
                {
                    Author = author,
                    Body = problem.Body,
                    CreateTime = problem.CreateTime,
                    Id = problem.Id,
                    Reward = problem.Reward,
                    Title = problem.Title,
                    Attachment=problem.Attachment,
                    CommentCount= commentcount,
                    BestKindHearted=bestKindHearted
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
                item.Author = new UserModel { Id = comment.Author.Id, Name = comment.Author.Name };
                item.Body = comment.Body;
                item.CreateTime = comment.CreateTime;
                item.Floor = comment.Floor;
                item.Id = comment.Id;
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
            var bestkind = await _context.Users.SingleOrDefaultAsync(x => x.Id == problem.RewardBestId.ToString());
            var commentcount = await _context.Comments.CountAsync(x=>x.ProblemId==id);
            var author = new UserModel {Id=problem.Author.Id,Name=problem.Author.Name };
            var bestKindHearted = new UserModel();
            if (bestkind != null)
            {
                bestKindHearted.Id = bestkind.Id;
                bestKindHearted.Name = bestkind.Name;
            }

            var model = new ItemModel
            {
                Author=author,
                Body = problem.Body,
                CreateTime = problem.CreateTime,
                Id = problem.Id,
                Reward = problem.Reward,
                Title = problem.Title,
                Attachment=problem.Attachment,
                CommentCount=commentcount,
                BestKindHearted= bestKindHearted
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
                Reward = 0,
                Attachment = attachment,
               
            };
            if (model.Reward.HasValue)
            {
                problem.Reward = model.Reward.Value;
            }

            await _context.Problems.AddAsync(problem);
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
            var commentCount = await _context.Comments.Where(x => x.ProblemId == problemId).CountAsync();
            var comment = new Comment
            {
                Body=model.Body,
                //CreateTime=DateTime.Now,
                ProblemId=problemId,
                UserId=author.Id,
                Floor=commentCount+1
            };
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return new ViewModel.Shared.Comment.ItemModel
            {
                Body = comment.Body,
                Author=new UserModel { Id=author.Id,Name=author.Name},
                CreateTime = comment.CreateTime,
                Floor=comment.Floor
            };

        }

        /// <summary>
        /// 酬谢评论
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public async Task<int> Reward(int commentId)
        {
            var comment = await _context.Comments.Include(x=>x.Author).ThenInclude(x=>x.CreditHistory).SingleOrDefaultAsync(x=>x.Id==commentId);
            var problem = await _context.Problems.Where(x => x.Id == comment.ProblemId).SingleOrDefaultAsync();
            var currentUserId =(await GetCurrentUser()).Id;

            //不能自己酬谢自己
            if (comment.Author.Id == currentUserId)
            {
                throw new Exception(string.Format(
                    "不能自己（ID={0}）酬谢自己。酬谢的CommentId={1}",
                    currentUserId, commentId));
            }

            //不能在别人的求助上酬谢
            if (problem.UserId != currentUserId)
            {
                throw new Exception(string.Format(
                    "用户Id={0}试图在他不是作者的求助（Id={1}）上进行酬谢。酬谢的CommentId={2}"
                    , currentUserId, problem.UserId, commentId));
            }

            comment.BeRewarded(problem);

            await _context.SaveChangesAsync();

            return problem.Reward;
        }

        public async Task UpdatePicture(int problemId, string attachment)
        {
            var problem = await _context.Problems.SingleOrDefaultAsync(x=>x.Id==problemId);
            problem.Attachment = attachment;
            _context.Problems.Update(problem);
            await _context.SaveChangesAsync();
        }
        public async Task Cancel(int problemId)
        {

            var problem = await _context.Problems.Include(x=>x.Author).ThenInclude(x=>x.CreditHistory).SingleOrDefaultAsync(x => x.Id == problemId);
            problem.Cancel();
            _context.Problems.Remove(problem);
            await _context.SaveChangesAsync();
        }

    }

}
