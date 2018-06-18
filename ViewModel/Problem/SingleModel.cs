using HELP.Service.ViewModel.Problem.Single;
using System.ComponentModel.DataAnnotations;

namespace HELP.Service.ViewModel.Problem
{
	public class SingleModel
	{
		[Required]
		public string Body { get; set; }

		public ItemModel Item { get; set; }
	}
}