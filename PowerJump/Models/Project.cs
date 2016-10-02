using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerJump.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}