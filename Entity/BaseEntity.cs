using System;
using System.Collections.Generic;
using System.Text;

namespace HELP.BLL.Entity
{
    public class BaseEntity
    {
        private DateTime _CreateTime;
        public BaseEntity()
        {
            SetCreateTime();
        }
        public int Id { get; set; }

        public DateTime CreateTime { get { return _CreateTime; } }

        internal virtual void SetCreateTime()
        {
            _CreateTime = DateTime.Now;
        }
    }
}
