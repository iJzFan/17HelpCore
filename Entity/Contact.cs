using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HELP.BLL.Entity
{
	public class Contact
	{
		[Key]
		[ForeignKey("User")]
		public string UserId { get; set; }
		public string QQ { get; set; }
		public string WeChat { get; set; }
		public string Telephone { get; set; }
		public string Other { get; set; }
		public virtual User User { get; set; }

	}
}
