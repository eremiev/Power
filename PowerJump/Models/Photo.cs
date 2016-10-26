using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerJump.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Path { get; set; }

        public int GalleryId { get; set; }
        public virtual Gallery Gallery { get; set; }
    }
}