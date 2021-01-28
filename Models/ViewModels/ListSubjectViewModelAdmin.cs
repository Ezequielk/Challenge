using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Models.ViewModels
{
    public class ListSubjectViewModelAdmin : ListSubjectViewModel
    {
        [Required(ErrorMessage = "Teacher is required")]
        [Display(Name = "Teacher*")]
        public int id_teacher { get; set; }
    }
}