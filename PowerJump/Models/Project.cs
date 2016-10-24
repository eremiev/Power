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
        [Display(Name = "Име")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        //[Display(Name = "Снимка")]
        //[DataType(DataType.Upload)]
        //HttpPostedFileBase FileName { get; set; }
        
    }
}