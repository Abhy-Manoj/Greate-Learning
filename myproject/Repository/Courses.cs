using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using myproject.Models;

public class CourseDataAccess
{
    private readonly string _connectionString = "Data Source=SYSLP1468;Initial Catalog=MyProjectDatabase;Integrated Security=True";

    public CourseDataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Courses GetCourseById(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPS_Course", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Courses
                        {
                            Id = (int)reader["Id"],
                            CourseName = reader["CourseName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Duration = reader["Duration"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            VideoUrl = reader["VideoUrl"].ToString()
                        };
                    }
                }
            }
        }
        return null;
    }

    public List<Courses> GetAllCourses()
    {
        List<Courses> courses = new List<Courses>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Courses", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        courses.Add(new Courses
                        {
                            Id = (int)reader["Id"],
                            CourseName = reader["CourseName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Duration = reader["Duration"].ToString(),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            VideoUrl = reader["VideoUrl"].ToString()
                        });
                    }
                }
            }
        }

        return courses;
    }

    public void InsertCourse(Courses course)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPI_Course", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Description", course.Description);
                command.Parameters.AddWithValue("@Duration", course.Duration);
                command.Parameters.AddWithValue("@ImageUrl", course.ImageUrl);
                command.Parameters.AddWithValue("@VideoUrl", course.VideoUrl);
                command.Parameters.AddWithValue("@Count", course.Count);

                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateCourse(Courses course)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPU_Course", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", course.Id);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Description", course.Description);
                command.Parameters.AddWithValue("@Duration", course.Duration);
                command.Parameters.AddWithValue("@ImageUrl", course.ImageUrl);
                command.Parameters.AddWithValue("@VideoUrl", course.VideoUrl);
                command.Parameters.AddWithValue("@Count", course.Count);

                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteCourse(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPD_Course", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
