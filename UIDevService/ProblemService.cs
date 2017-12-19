using HELP.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Text;
using HELP.Service.ViewModel.Problem;
using HELP.GlobalFile.Global.Story;
using System.Threading.Tasks;

namespace HELP.Service.UIDevService
{
    public class ProblemService : IProblemService
    {
        public IndexModel Get()
        {
            return new IndexModel
            {
                Items = new List<ItemModel>()
                {
                    new ItemModel
                    {
                        AuthorId = 1,
                        AuthorName = User.DK_UserName,
                        Body = Problem.PhoneGap_Body,
                        CreatedTime = Problem.PhoneGap_CreateTime,
                        Id = Problem.PhoneGap_Id,
                        Reward = Problem.PhoneGap_Reward,
                        Title = Problem.PhoneGap_Title
                    },
                    new ItemModel
                    {
                        AuthorId = 1,
                        AuthorName = User.DK_UserName,
                        Body = Problem.WeChat_Body,
                        CreatedTime = Problem.WeChat_CreateTime,
                        Id = Problem.WeChat_Id,
                        Reward = Problem.WeChat_Reward,
                        Title = Problem.WeChat_Title
                    },
                    new ItemModel
                    {
                        AuthorId = 1,
                        AuthorName = User.DK_UserName,
                        Body = Problem.SSCE_Body,
                        CreatedTime = Problem.SSCE_CreateTime,
                        Id = Problem.SSCE_Id,
                        Reward = Problem.SSCE_Reward,
                        Title = Problem.SSCE_Title
                    },
                    new ItemModel
                    {
                        AuthorId = 1,
                        AuthorName = User.DK_UserName,
                        Body = Problem.WebGrease_Body,
                        CreatedTime = Problem.WebGrease_CreateTime,
                        Id = Problem.WebGrease_Id,
                        Reward = Problem.WebGrease_Reward,
                        Title = Problem.WebGrease_Title
                    },
                    new ItemModel
                    {
                        AuthorId = 2,
                        AuthorName = User.yezi_UserName,
                        Body = Problem.Install_Body,
                        CreatedTime = Problem.Install_CreateTime,
                        Id = Problem.Install_Id,
                        Reward = Problem.Install_Reward,
                        Title = Problem.Install_Title
                    }
                }
            };
        }

        Task<IndexModel> IProblemService.Get()
        {
            throw new NotImplementedException();
        }

        Task IProblemService.Save(NewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
