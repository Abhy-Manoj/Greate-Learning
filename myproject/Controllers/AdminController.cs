using myproject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace myproject.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Homepage()
        {
            string username = (string)Session["Username"];
            ViewBag.Username = username;
            return View();

        }


        public ActionResult Userdetails()
        {
            List<ViewUsers> employees = new List<ViewUsers>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Signup";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ViewUsers employee = new ViewUsers();
                    employee.Email = reader["Email"].ToString();
                    employee.Username = reader["Username"].ToString();
                    employee.FirstName = reader["FirstName"].ToString();
                    employee.LastName = reader["LastName"].ToString();
                    employee.DateOfBirth = reader["DateOfBirth"].ToString();
                    employee.Gender = reader["Gender"].ToString();
                    employee.Phone = reader["Phone"].ToString();
                    employee.Id = Convert.ToInt32(reader["Id"]);

                    employees.Add(employee);
                }

                reader.Close();
            }

            return View(employees);
        }

        public ActionResult Admindetails()
        {
            List<RegisteredEmployees> Registered = new List<RegisteredEmployees>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Username, Email, Password,Id FROM Login";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RegisteredEmployees registered = new RegisteredEmployees();
                    registered.Username = reader["Username"].ToString();
                    registered.Email = reader["Email"].ToString();
                    registered.Password = reader["Password"].ToString();
                    registered.Id = Convert.ToInt32(reader["Id"]);

                    Registered.Add(registered);
                }

                reader.Close();
            }

            return View(Registered);
        }

        public ActionResult AdminPassword()
        {
            List<RegisteredEmployees> Registered = new List<RegisteredEmployees>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Username, Email, Password,Id FROM Login";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RegisteredEmployees registered = new RegisteredEmployees();
                    registered.Username = reader["Username"].ToString();
                    registered.Email = reader["Email"].ToString();
                    registered.Password = reader["Password"].ToString();
                    registered.Id = Convert.ToInt32(reader["Id"]);

                    Registered.Add(registered);
                }

                reader.Close();
            }

            return View(Registered);
        }

        public ActionResult DeleteUser(int id)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SPD_UserRegistration", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return RedirectToAction("RegisteredEmployees");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "User not found.";
                            return View("Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while deleting the user.";
                return View("Error");
            }
        }




        public ActionResult Courses()
        {
            List<Courses> viewcourse = new List<Courses>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT CourseName, Description, Duration,Id, ImageUrl, VideoUrl FROM Courses";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Courses course = new Courses();
                    course.CourseName = reader["CourseName"].ToString();
                    course.Description = reader["Description"].ToString();
                    course.Duration = reader["Duration"].ToString();
                    course.ImageUrl = reader["ImageUrl"].ToString();
                    course.VideoUrl = reader["VideoUrl"].ToString();
                    course.Id = Convert.ToInt32(reader["Id"]);
                    viewcourse.Add(course);
                }

                reader.Close();
            }

            return View(viewcourse);
        }

    }
}

