using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using myproject.Models;

public class SCourseController : Controller
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    public ActionResult UserCourses()
    {
        string currentUsername = (string)Session["Username"];

        int currentUserId = -1;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand getUserIdCmd = new SqlCommand("SELECT Id FROM Signup WHERE Username = @Username", connection))
            {
                getUserIdCmd.Parameters.AddWithValue("@Username", currentUsername);
                var userIdObj = getUserIdCmd.ExecuteScalar();
                if (userIdObj != null)
                {
                    if (int.TryParse(userIdObj.ToString(), out currentUserId))
                    {
                        using (SqlCommand cmd = new SqlCommand("SPS_SelectedCourse", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserId", currentUserId);

                            List<SelectedCourse> userCourses = new List<SelectedCourse>();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    SelectedCourse course = new SelectedCourse();
                                    course.CourseId = (int)reader["CourseId"];
                                    course.CourseName = reader["CourseName"].ToString();
                                    course.Description = reader["Description"].ToString();
                                    course.Duration = reader["Duration"].ToString();
                                    course.ImageUrl = reader["ImageUrl"].ToString();

                                    userCourses.Add(course);
                                }
                            }

                            return View(userCourses);
                        }
                    }
                }
            }
        }


        return RedirectToAction("ErrorPage");
    }


    public ActionResult UnjoinCourse(int courseId)
    {
        string currentUsername = (string)Session["Username"];

        int currentUserId = -1;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand getUserIdCmd = new SqlCommand("SELECT Id FROM Signup WHERE Username = @Username", connection))
            {
                getUserIdCmd.Parameters.AddWithValue("@Username", currentUsername);
                var userIdObj = getUserIdCmd.ExecuteScalar();
                if (userIdObj != null)
                {
                    if (int.TryParse(userIdObj.ToString(), out currentUserId))
                    {
                        using (SqlCommand cmd = new SqlCommand("SP_UnjoinCourse", connection))
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", courseId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        return RedirectToAction("UserCourses");
    }

    public Courses GetCourseDetails(int courseId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand cmd = new SqlCommand("SPS_Course", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", courseId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Courses course = new Courses();
                        course.Id = (int)reader["Id"];
                        course.CourseName = reader["CourseName"].ToString();
                        course.Description = reader["Description"].ToString();
                        course.Duration = reader["Duration"].ToString();
                        course.ImageUrl = reader["ImageUrl"].ToString();
                        course.VideoUrl = reader["VideoUrl"].ToString();

                        return course;
                    }
                }
            }
        }

        return null;
    }


    public ActionResult CourseDetails(int courseId)
    {
        var course = GetCourseDetails(courseId);

        if (course != null)
        {            
            return View("CourseDetails", course);
        }

        return RedirectToAction("CourseNotFound");
    }

}
