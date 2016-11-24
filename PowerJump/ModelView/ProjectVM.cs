using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PowerJump.Models;

namespace PowerJump.ModelView
{
    public class ProjectVM
    {
        public Project ProjectModel { get; set; }
        public ProjectLocales ProjectLocalesModel { get; set; }

       // public SelectList Genres { get; set; }
    }
}