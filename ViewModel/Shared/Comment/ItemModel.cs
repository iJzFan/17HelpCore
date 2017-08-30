using System;
using System.Collections.Generic;
using System.Text;

namespace HELP.Service.ViewModel.Shared.Comment
{
    public class ItemModel
    {
        public DateTime CreateTime { get; set; }
        public string Body { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
