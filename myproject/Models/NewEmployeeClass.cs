using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace myproject.Models
{
    public class NewEmployeeClass
    {
        public int Employee_id { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        public string Employee_name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }



    }
}