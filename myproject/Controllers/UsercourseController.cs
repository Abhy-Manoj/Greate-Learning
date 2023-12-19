using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using myproject.Models;

public class UserCourseController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    //Get the enrolled users count
    public int GetEnrollmentCount(int courseId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("SP_GetCount", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                return (int)cmd.ExecuteScalar();
            }
        }
    }

    //Get the list of courses
    public ActionResult ListCourses()
    {
        List<Courses> courses = new List<Courses>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("SP_Course", connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Courses course = new Courses();
                        course.Id = (int)reader["Id"];
                        course.CourseName = reader["CourseName"].ToString();
                        course.ImageUrl = reader["ImageUrl"].ToString();
                        course.Count = GetEnrollmentCount(course.Id);
                        courses.Add(course);
                    }
                }
            }
        }

        return View(courses);
    }

    //View the enrolled users for particular courses
    public ActionResult ViewEnrolledUsers(int courseId)
    {
        List<UserModel> enrolledUsers = new List<UserModel>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("SP_EnrolledUsers", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseId", courseId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UserModel user = new UserModel();
                        user.FirstName = reader["FirstName"].ToString();
                        user.LastName = reader["LastName"].ToString();
                        user.Username = reader["Username"].ToString();

                        enrolledUsers.Add(user);
                    }
                }
            }
        }

        return View(enrolledUsers);
    }
}
