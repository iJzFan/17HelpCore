using HELP.Service.ViewModel.Shared;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HELP.Service.ViewModel.Register
{
	public class HomeModel
	{
		[Required]
		[DisplayName("用户名")]
		[StringLength(20, MinimumLength = 4, ErrorMessage = "*用户名长度只能是4~20个字符")]
		public string UserName { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 4, ErrorMessage = "*密码长度只能是4~20个字符")]
		[DisplayName("密码")]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "* 确认密码和密码不一致")]
		[DisplayName("确认密码")]
		public string ConfirmPassword { get; set; }

		public ImageCodeModel ImageCode { get; set; }
	}
}