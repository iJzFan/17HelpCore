using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HELP.Service.ViewModel.Problem
{
	public class NewModel
	{
		[Required]
		[StringLength(255)]
		[Display(Name = "标题")]
		public string Title { get; set; }

		[Required]
		[Display(Name = "问题描述")]
		public string Body { get; set; }

		public IFormFile Picture { get; set; }

		[Display(Name = "悬赏时间币")]
		//TODO: 小数的验证也不能在前端进行
		[Range(0, int.MaxValue, ErrorMessage = "* 只能为大于零的正整数")]
		//TODO:没有前端验证效果[Number]
		public int? Reward { get; set; }
	}
}