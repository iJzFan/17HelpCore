using System;
using System.Collections.Generic;
using System.Text;

namespace HELP.Service.ViewModel.Shared.Comment
{
    public class ItemModel
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string Body { get; set; }
        public UserModel Author { get; set; }
    }
}
