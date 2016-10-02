using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerJump.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}