using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using myproject.Models;

public class AdminDataAccess
{
    private readonly string _connectionString = "Data Source=SYSLP1468;Initial Catalog=MyProjectDatabase;Integrated Security=True";

    public AdminDataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

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

    public List<Admin> GetAllAdmins()
    {
        List<Admin> admins = new List<Admin>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Login", connection))
            {
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
