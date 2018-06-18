using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HELP.Service.ViewModel.Contact
{
	public class RecordModel
	{
		[StringLength(12, MinimumLength = 5)]
		public string QQ { get; set; }

		public string WeChat { get; set; }
		public string Telephone { get; set; }

		[DisplayName("其他说明")]
		[StringLength(255)]
		public string Other { get; set; }
	}
}