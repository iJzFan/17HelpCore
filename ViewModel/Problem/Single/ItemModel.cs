using System;
using System.Collections.Generic;
using System.Text;

namespace HELP.Service.ViewModel.Problem.Single
{
    public class ItemModel
    {
        public DateTime CreateTime { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Reward { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
