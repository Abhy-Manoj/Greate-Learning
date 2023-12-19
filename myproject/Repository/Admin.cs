using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using myproject.Models;

public class AdminDataAccess
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    public AdminDataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    //Get admin details by id
    public Admin GetAdminById(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPS_AdminRegistration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Admin
                        {
                            Id = (int)reader["Id"],
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                    }
                }
            }
        }
        return null;
    }

    //Get admin details
    public List<Admin> GetAllAdmins()
    {
        List<Admin> admins = new List<Admin>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SP_Login", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        admins.Add(new Admin
                        {
                            Id = (int)reader["Id"],
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }
        }

        return admins;
    }

    //Add new admin
    public void InsertAdmin(Admin admin)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPI_AdminRegistration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", admin.Username);
                command.Parameters.AddWithValue("@Password", admin.Password);
                command.Parameters.AddWithValue("@Email", admin.Email);

                command.ExecuteNonQuery();
            }
        }
    }

    //Update admin details
    public void UpdateAdmin(Admin admin)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPU_AdminRegistration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", admin.Id);
                command.Parameters.AddWithValue("@Username", admin.Username);
                command.Parameters.AddWithValue("@Email", admin.Email);

                command.ExecuteNonQuery();
            }
        }
    }

    //Update the password of the admin
    public void UpdateAdminPass(Admin admin)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPU_AdminPassword", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", admin.Id);
                command.Parameters.AddWithValue("@Password", admin.Password);

                command.ExecuteNonQuery();
            }
        }
    }

    //View enrollment requests
    public List<PendingEnrollmentRequest> GetPendingEnrollmentRequests()
    {
        List<PendingEnrollmentRequest> pendingRequests = new List<PendingEnrollmentRequest>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SP_GetEnrollmentRequests", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pendingRequests.Add(new PendingEnrollmentRequest
                        {
                            RequestId = (int)reader["RequestId"],
                            CourseId = (int)reader["CourseId"],
                            Username = reader["Username"].ToString(),
                            CourseName = reader["CourseName"].ToString(),
                            Status = reader["Status"].ToString()
                        });
                    }
                }
            }
        }

        return pendingRequests;
    }

    //Delete the admin
    public void DeleteAdmin(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPD_AdminRegistration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
