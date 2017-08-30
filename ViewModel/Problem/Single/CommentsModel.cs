using System.Collections.Generic;
using HELP.Service.ViewModel.Shared.Comment;

namespace HELP.Service.ViewModel.Problem.Single
{
    public class CommentsModel
    {
        public IList<Shared.Comment.ItemModel> List { get; set; }
    }
}
