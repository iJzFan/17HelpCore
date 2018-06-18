using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HELP.Service.ViewModel.Shared
{
	public class ImageCodeModel
	{
		[Required]
		[DisplayName("验证码")]
		[StringLength(4, MinimumLength = 4, ErrorMessage = "* 验证码长度为｛0｝")]
		public string InputImageCode { get; set; }

		public ImageCodeError ImageCodeError { get; set; }
	}

	public enum ImageCodeError
	{
		NoError = 0,
		Wrong = 1,
		Expired = 2
	}
}