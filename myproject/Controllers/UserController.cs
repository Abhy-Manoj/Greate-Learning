using myproject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace myproject.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Userhome()
        {
            string username = (string)Session["Username"];
            ViewBag.Username = username;
            return View();
        }

        public ActionResult ViewUserProfile(int userId)
        {
            ViewUsers user = new ViewUsers();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SPS_UserRegistration";

                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Id", userId);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user.Email = reader["Email"].ToString();
                    user.Username = reader["Username"].ToString();
                    user.FirstName = reader["FirstName"].ToString();
                    user.LastName = reader["LastName"].ToString();
                    user.DateOfBirth = reader["DateOfBirth"].ToString();
                    user.Gender = reader["Gender"].ToString();
                    user.Phone = reader["Phone"].ToString();
                    user.DateOfBirth = reader["DateOfBirth"].ToString();
                    user.State = reader["State"].ToString();
                    user.City = reader["City"].ToString();
                    user.Id = Convert.ToInt32(reader["Id"]);
                }

                reader.Close();
            }

            return View("ViewUserProfile", user);
        }


        public ActionResult ViewTasks()
        {
            List<Courses> tasks = new List<Courses>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            string currentUser = (string)Session["Username"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, CourseName, Description, Duration, ImageUrl FROM Courses";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", currentUser);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Courses courses = new Courses();
                    courses.Id = Convert.ToInt32(reader["Id"]);
                    courses.CourseName = reader["CourseName"].ToString();
                    courses.Description = reader["Description"].ToString();
                    courses.Duration = reader["Duration"].ToString();
                    courses.ImageUrl = reader["ImageUrl"].ToString();

                    tasks.Add(courses);
                }

                reader.Close();
            }

            return View(tasks);
        }
                     
    }

}




