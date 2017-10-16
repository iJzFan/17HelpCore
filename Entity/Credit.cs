using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HELP.BLL.Entity
{
    public class Credit: BaseEntity
    {


        private int _balance;
        /// <summary>
        /// 每次生成一条明细计算出来的用户当前剩余时间币
        /// </summary>
        public int Balance
        {
            get { return _balance; }
        }

        /// <summary>
        /// 当前明细的时间币变化量（增加为正数/减少为负数）
        /// </summary>
        public int Count { get; set; }
        public string Description { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual void SetBalance()
        {
            _balance = User.Creditpoints + Count;

            if (_balance < 0)
            {
                throw new Exception(
                    string.Format("用户id={0}的现有时间币{1}，消耗时间币{2}，结算时间币小于0，Description为：{3}",
                    User.Id, User.Creditpoints, Count, Description));
            }

            User.Creditpoints = _balance;
        }
    }
}
