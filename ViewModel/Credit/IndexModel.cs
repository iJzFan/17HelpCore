using System;
using System.Collections.Generic;

namespace HELP.Service.ViewModel.Credit
{
	public class IndexModel
	{
		public IList<IndexItemModel> Items { get; set; }
	}

	public class IndexItemModel
	{
		public int Balance { get; set; }
		public int Count { get; set; }
		public DateTime CreateTime { get; private set; }
		public string Description { get; set; }
	}
}