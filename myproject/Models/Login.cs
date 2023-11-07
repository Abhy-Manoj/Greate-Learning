using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace myproject.Models
{
    public class Login
    {
        
        [Required(ErrorMessage = "Please Enter Your Username !")]
        [Display(Name = "Username :")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password !")]
        [Display(Name = "Password :")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int Id { get; set; }
    }
}