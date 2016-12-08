using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PowerJump.Models;

namespace PowerJump.ModelView
{
    public class ProjectsAndEventsVM
    {
        public List<ICollection<ProjectLocales>> ProjectsLocalesModel { get; set; }
        public List<Event> EventModel { get; set; }
        public ProjectLocales ProjectModel { get; set; }
        public Event ev { get; set; }
    }
}