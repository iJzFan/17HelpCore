using HELP.Service.ViewModel.Problem.Single;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HELP.Service.ViewModel.Problem
{
    public class SingleModel
    {
        [Required]
        public string Body { get; set; }

        public ItemModel Item { get; set; }
    }
}
