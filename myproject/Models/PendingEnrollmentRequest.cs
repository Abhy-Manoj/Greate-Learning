using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myproject.Models
{
    public class PendingEnrollmentRequest
    {
        public int RequestId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }

    }
}