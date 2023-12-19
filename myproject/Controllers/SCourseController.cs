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

    //Get the details of courses selected by the user
    public ActionResult UserCourses()
    {
        string currentUsername = (string)Session["Username"];

        int currentUserId = -1;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand getUserIdCmd = new SqlCommand("SP_SignupUser", connection))

            {
                getUserIdCmd.CommandType = CommandType.StoredProcedure;
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

    //Unjoin the course
    public ActionResult UnjoinCourse(int courseId)
    {
        string currentUsername = (string)Session["Username"];

        int currentUserId = -1;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand getUserIdCmd = new SqlCommand("SP_SignupUser", connection))
            {
                getUserIdCmd.CommandType = CommandType.StoredProcedure;
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
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            //Increase the course count by 1 when unjoining a course
            using (SqlCommand cmd = new SqlCommand("SP_ChangeEnrollmentCount", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseId", courseId);
                cmd.Parameters.AddWithValue("@ChangeAmount", 1); // Increment by 1 when unjoining
                cmd.ExecuteNonQuery();
            }
        }

        return RedirectToAction("UserCourses");
    }

    //Get the details of course by id
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
                        course.Count = (int)reader["Count"];
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
