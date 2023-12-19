using myproject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace myproject.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminDataAccess adminDataAccess = new AdminDataAccess(ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString);

        public ActionResult Homepage()
        {
            string username = (string)Session["Username"];
            ViewBag.Username = username;
            return View();

        }

        //Get the user details from database
        public ActionResult Userdetails()
        {
            List<ViewUsers> employees = new List<ViewUsers>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {                
                SqlCommand command = new SqlCommand("SP_Signup", connection);
                command.CommandType = CommandType.StoredProcedure;

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

        //Get the admin details from database
        public ActionResult Admindetails()
        {
            List<RegisteredEmployees> Registered = new List<RegisteredEmployees>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {           
                SqlCommand command = new SqlCommand("SP_Login", connection);
                command.CommandType = CommandType.StoredProcedure;
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

        //To change the admin password
        public ActionResult AdminPassword()
        {
            List<RegisteredEmployees> Registered = new List<RegisteredEmployees>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SP_Login", connection);
                command.CommandType = CommandType.StoredProcedure;

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

        //To delete the user
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

        //View different courses available
        public ActionResult Courses()
        {
            List<Courses> viewcourse = new List<Courses>();

            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string storedProcedureName = "SP_Course";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Courses course = new Courses();
                            course.CourseName = reader["CourseName"].ToString();
                            course.Description = reader["Description"].ToString();
                            course.Duration = reader["Duration"].ToString();
                            string base64Image = reader["ImageUrl"].ToString();
                            // Convert base64 strings to bytes
                            byte[] imageBytes = Convert.FromBase64String(base64Image);
                            course.ImageUrl = Convert.ToBase64String(imageBytes);
                            course.VideoUrl = reader["VideoUrl"].ToString();
                            course.Count = Convert.ToInt32(reader["Count"]);
                            course.Id = Convert.ToInt32(reader["Id"]);
                            viewcourse.Add(course);
                        }
                        reader.Close();
                    }
                }

            }

            return View(viewcourse);
        }


        //View enrollment requests
        public ActionResult ViewPendingEnrollmentRequests()
        {
            List<PendingEnrollmentRequest> pendingRequests = adminDataAccess.GetPendingEnrollmentRequests();
            return View(pendingRequests);
        }

        //Approve the enrollment requests
        public ActionResult ApproveEnrollmentRequest(int requestId, int courseId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand approveRequestCmd = new SqlCommand("SP_ApproveEnrollmentRequest", connection))
                {
                    approveRequestCmd.CommandType = CommandType.StoredProcedure;
                    approveRequestCmd.Parameters.AddWithValue("@RequestId", requestId);
                    approveRequestCmd.ExecuteNonQuery();
                }
                //Decrease the course count
                using (SqlCommand cmd = new SqlCommand("SP_ChangeEnrollmentCount", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    cmd.Parameters.AddWithValue("@ChangeAmount", -1); // Decrement by 1 when joining
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("EnrollmentRequests");
            }

        }

    }
}

