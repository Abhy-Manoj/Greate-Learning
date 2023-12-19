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

        //View the particular user details
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

        //View the courses available for the user
        public ActionResult ViewCourse()
        {
            int currentUserId = (int)Session["UserId"];
            int userId = currentUserId;

            List<Courses> tasks = new List<Courses>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            string currentUser = (string)Session["Username"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "SP_UserCourse";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", currentUser);
                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Courses courses = new Courses();
                            courses.Id = Convert.ToInt32(reader["Id"]);
                            courses.CourseName = reader["CourseName"].ToString();
                            courses.Description = reader["Description"].ToString();
                            courses.Duration = reader["Duration"].ToString();
                            courses.ImageUrl = reader["ImageUrl"].ToString();
                            courses.Count = Convert.ToInt32(reader["Count"]);

                            tasks.Add(courses);
                        }
                    }
                }
            }

            return View(tasks);
        }

        //Change the user password
        public ActionResult UserPassword()
        {
            List<ViewUsers> user = new List<ViewUsers>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "SP_Signup";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ViewUsers User = new ViewUsers();
                        User.Email = reader["Email"].ToString();
                        User.Username = reader["Username"].ToString();
                        User.FirstName = reader["FirstName"].ToString();
                        User.LastName = reader["LastName"].ToString();
                        User.DateOfBirth = reader["DateOfBirth"].ToString();
                        User.Gender = reader["Gender"].ToString();
                        User.Phone = reader["Phone"].ToString();
                        User.DateOfBirth = reader["DateOfBirth"].ToString();
                        User.State = reader["State"].ToString();
                        User.City = reader["City"].ToString();
                        User.Password = reader["Password"].ToString();
                        User.ConfirmPassword = reader["ConfirmPassword"].ToString();
                        User.Id = Convert.ToInt32(reader["Id"]);
                    }

                    reader.Close();
                }
            }

            return View(user);
        }

    }

}




