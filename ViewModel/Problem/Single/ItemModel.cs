using HELP.Service.ViewModel.Shared;
using System;

namespace HELP.Service.ViewModel.Problem.Single
{
	public class ItemModel
	{
		public DateTime CreateTime { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public int CommentCount { get; set; }
		public int Reward { get; set; }
		public bool? HasReward { get; set; }
		public string Attachment { get; set; }
		public UserModel Author { get; set; }
		public UserModel BestKindHearted { get; set; }
	}
}