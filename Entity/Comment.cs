using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HELP.BLL.Entity
{
    public class Comment: BaseEntity
    {

        public  string Body { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        [ForeignKey("ProblemId")]
        public int ProblemId { get; set; }

        public int Floor { get; set; }

        public virtual User Author { get; set; }

        [NotMapped]
        public virtual Problem Problem { get; set; }

        public virtual void BeRewarded()
        {
            Credit credit = new Credit
            {
                Count = Problem.Reward,
                User = Author,
                Description = string.Format(
                    "获得酬谢：<a href='/Problem/{0}' target='_blank'>{1}</a>",
                    Problem.Id, Problem.Title)
            };
            credit.SetBalance();
            Author.CreditHistory.Add(credit);

            if (Problem.RewardBest != null)
            {
                throw new Exception(string.Format(
                    "不能在求助（Id={0}）上重复的进行酬谢。已酬谢评论Id={1}，现试图酬谢的Id={2}",
                    Problem.Id, Problem.RewardBest.Id, Id));
            }
            Problem.RewardBest = this;
        }

    }
}
