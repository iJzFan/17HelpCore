using HELP.Service.ViewModel.Problem;
using HELP.Service.ViewModel.Problem.Single;
using System;
using System.Threading.Tasks;

namespace HELP.Service.ServiceInterface
{
    public interface IProblemService
    {
        Task<(IndexModel index,int count)> Get(int id,int pagesize);

        Task Save(NewModel model,string atattachment);

        Task<ItemModel> GetItem(int id);
        Task<CommentsModel> GetComments(int id);
        Task<ViewModel.Shared.Comment.ItemModel> SaveComment(SingleModel model, int problemId);
        Task<int> Reward(int commentId);
        Task UpdatePicture(int problemId, string attachment);
        Task Cancel(int problemId);
    }
}
