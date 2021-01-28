using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Models.ViewModels
{
    public class ListTeacherViewModel
    {
        public int id_teacher { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First name*")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last name*")]
        public string last_name { get; set; }

        [Display(Name = "active")]
        public bool active { get; set; }
    }
}