using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerJump.Models
{
    public abstract class Gallery
    {
        public int GalleryId { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}