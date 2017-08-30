

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HELP.BLL.Entity
{
    public class Problem: BaseEntity
    {


        public string Attachment { get; set; }


        public int? RewardBestId { get; set; }

        public virtual Comment RewardBest { get; protected internal set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public int Reward { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }


        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Commnets { get; set; }

        public virtual void Publish()
        {
            if (Reward > 0)
            {
                Credit credit = new Credit
                {
                    Count = 0 - Reward,
                    User = Author
                };
                //TODO
                credit.Description =
                    string.Format("悬赏：<a href='/Problem/{0}' target='_blank'>{1}</a>",
                    Id, Title);
                credit.SetBalance();

                Author.CreditHistory.Add(credit);
            }
            else
            {

            }
        }
    }
}
