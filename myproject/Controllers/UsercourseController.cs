using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using myproject.Models;

public class UserCourseController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    public ActionResult ListCourses()
    {
        List<Courses> courses = new List<Courses>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT Id, CourseName, ImageUrl FROM Courses", connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Courses course = new Courses();
                        course.Id = (int)reader["Id"];
                        course.CourseName = reader["CourseName"].ToString();
                        course.ImageUrl = reader["ImageUrl"].ToString();
                        courses.Add(course);
                    }
                }
            }
        }

        return View(courses);
    }

    public ActionResult ViewEnrolledUsers(int courseId)
    {
        List<UserModel> enrolledUsers = new List<UserModel>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT U.FirstName, U.LastName, U.Username FROM Signup U " +
                                                   "INNER JOIN S_Course SC ON U.Id = SC.uid " +
                                                   "WHERE SC.cid = @CourseId", connection))
            {
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
