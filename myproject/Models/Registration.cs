using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;



namespace myproject.Models
{
    public class Registration
    {
        [Display(Name = "id")]
        public int Id { get; set; }

        [Display(Name = "First name")]
        public String FirstName { get; set; }

        [Display(Name = "Last name")]
        public String LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public String DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public String Gender { get; set; }

        [Display(Name = "Phone")]
        public String Phone { get; set; }


        [Display(Name = "E-mail")]
        public String Email { get; set; }

        [Display(Name = "State")]
        public String State { get; set; }

        [Display(Name = "City")]
        public String City { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public String Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Display(Name = "Confirm password")]
        public String ConfirmPassword { get; set; }
        public string HashedPassword { get; set; }
    }
}