using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myproject.Models
{

    public class Courses
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string VideoUrl { get; set; }
        public HttpPostedFileBase VideoFile { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public int Count { get; set; }
    }

}