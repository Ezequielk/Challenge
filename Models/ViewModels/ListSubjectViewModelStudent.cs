using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Challenge.Models.ViewModels
{
    public class ListSubjectViewModelStudent : ListSubjectViewModel
    {
        public string teacher_name { get; set; }
        public int remaining_places { get; set; }
    }
}