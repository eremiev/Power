using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PowerJump.Models
{
    public class ProjectLocales
    {
        public int ID { get; set; }
        public string Locale { get; set; }

        [Required]
        [Display(Name = "Име")]
        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [StringLength(5000, MinimumLength = 3)]
        public string Description { get; set; }

        public virtual Project Project { get; set; }
    }
}