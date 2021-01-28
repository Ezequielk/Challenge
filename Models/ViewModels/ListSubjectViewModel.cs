using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Challenge.Models.ViewModels
{
    public class ListSubjectViewModel
    {
        public int id_subject { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name*")]
        public string name { get; set; }

        [Display(Name = "Description:")]
        public string description { get; set; }

        [Required(ErrorMessage = "Starting time is required")]
        [Display(Name = "From*")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm\:ss}")]
        public System.TimeSpan time_from { get; set; }

        [Required(ErrorMessage = "Ending time is required")]
        [Display(Name = "To*")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm\:ss}")]
        public System.TimeSpan time_to { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Display(Name = "Capacity*")]
        public int capacity { get; set; }
    }
}