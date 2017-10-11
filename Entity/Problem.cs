

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HELP.BLL.Entity
{
    public class Problem : BaseEntity
    {
        public string Attachment { get; set; }

        public int? RewardBestId { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public int Reward { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        #region Navigation property
        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Commnets { get; set; }

        public virtual Comment RewardBest { get; protected internal set; }
        #endregion

        #region Public method
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

        public virtual void Cancel()
        {
            //检测求助是否已结贴
            if (RewardBest != null)
            {
                throw new Exception(string.Format(
                    "求助（id={0}）已因回复（id={1}）酬谢给好心人（id={2}），不可撤销",
                    Author.Id, RewardBest.Id, RewardBest.Author.Id));
            }

            //返还悬赏分
            Credit credit = new Credit
            {
                Count = Reward,
                UserId = UserId,
                User=Author
            };
            credit.Description =
                string.Format("撤销悬赏：<a href='/Problem/{0}' target='_blank'>{1}</a>",
                Id, Title);
            credit.SetBalance();

            Author.CreditHistory.Add(credit);
        }
        #endregion
    }
}
