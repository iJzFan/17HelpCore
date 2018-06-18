using HELP.Service.ViewModel.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HELP.Service.ViewModel.Log
{
	public class OnModel
	{
		[Required]
		[DisplayName("用户名")]
		[StringLength(20, MinimumLength = 4, ErrorMessage = "*用户名长度只能是4~20个字符")]
		public string UserName { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 4, ErrorMessage = "*密码长度只能是4~20个字符")]
		[DisplayName("密码")]
		public string Password { get; set; }

		public ImageCodeModel ImageCode { get; set; }
	}
}