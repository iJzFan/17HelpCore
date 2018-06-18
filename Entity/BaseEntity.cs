using System;
using System.Collections.Generic;
using System.Text;

namespace HELP.BLL.Entity
{
    public class BaseEntity
    {
		public BaseEntity()
        {
            SetCreateTime();
        }
        public int Id { get; set; }

		public DateTime CreateTime { get; private set; }

		internal virtual void SetCreateTime()
        {
            CreateTime = DateTime.Now;
        }
    }
}
