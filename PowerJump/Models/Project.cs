using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PowerJump.Models
{
    [Table("Projects")]
    public class Project : Gallery
    {
        [Required]
        [Display(Name = "Име")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [StringLength(5000, MinimumLength = 3)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

    }
}