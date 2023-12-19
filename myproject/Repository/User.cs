using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using myproject.Models;

public class User
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

    public User(string connectionString)
    {
        _connectionString = connectionString;
    }

    //View the users
    public ViewUsers GetUserById(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPS_UserRegistration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ViewUsers
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Username = reader["Username"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Password = reader["Password"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = reader["DateOfBirth"].ToString(),
                            State = reader["State"].ToString(),
                            City = reader["City"].ToString(),
                        };
                    }
                }
            }
        }
        return null;
    }

    //All the users in the database
    public List<ViewUsers> GetAllUsers()
    {
        List<ViewUsers> users = new List<ViewUsers>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SELECT * FROM Signup", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new ViewUsers
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Username = reader["Username"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Password = reader["Password"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            DateOfBirth = reader["DateOfBirth"].ToString(),
                            State = reader["State"].ToString(),
                            City = reader["City"].ToString(),
                        });
                    }
                }
            }
        }

        return users;
    }

    //Add user
    public void InsertUser(ViewUsers user)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPI_UserRegistration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@State", user.State);
                command.Parameters.AddWithValue("@City", user.City);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Password", user.ConfirmPassword);
                command.ExecuteNonQuery();
            }
        }
    }

    //update the user details
    public void UpdateUser(ViewUsers user)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPU_UserRegistration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.ExecuteNonQuery();
            }
        }
    }

    //Update the password
    public void UpdateUserPass(ViewUsers user)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPU_UserPassword", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                command.Parameters.AddWithValue("@Password", hashedPassword);
                command.Parameters.AddWithValue("@ConfirmPassword", user.ConfirmPassword);
                command.Parameters.AddWithValue("@Id", user.Id);

                command.ExecuteNonQuery();
            }
        }
    }

    //Delete user
    public void DeleteUser(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand("SPD_UserRegistration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }
}
